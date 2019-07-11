using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Neuralm.Application.Interfaces;
using Neuralm.Application.Messages;
using Neuralm.Application.Messages.Requests;
using Neuralm.Application.Messages.Responses;
using Neuralm.Infrastructure.Interfaces;
using Neuralm.Infrastructure.MessageSerializers;
using Neuralm.Infrastructure.Networking;

namespace Neuralm.Client
{
    internal static class Program
    {
        private const int Port = 9999;
        private const string Host = "localhost";
        private const int MessageCount = 10_000;
        private const int ClientCount = 8;
        private const int TotalMessages = MessageCount * ClientCount;

        static async Task Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            List<Task> tasks = new List<Task>();
            for (int j = 0; j < ClientCount; j++)
            {
                Task clientTask = Task.Run(async () =>
                {
                    IMessageProcessor messageProcessor = new ClientMessageProcessor();
                    IMessageSerializer messageSerializer = new JsonMessageSerializer();
                    INetworkConnector networkConnector = new TcpNetworkConnector(messageSerializer, messageProcessor, Host, Port);
                    await networkConnector.ConnectAsync(CancellationToken.None);
                    networkConnector.Start();

                    MessageListener<AuthenticateResponse> loginResponseListener = new MessageListener<AuthenticateResponse>();
                    loginResponseListener.Subscribe(messageProcessor);
                    for (int i = 0; i < MessageCount; i++)
                    {
                        AuthenticateRequest loginRequest = new AuthenticateRequest("Mario", "password", "Name");
                        await networkConnector.SendMessageAsync(loginRequest, CancellationToken.None);
                        AuthenticateResponse loginResponse = await loginResponseListener.ReceiveMessageAsync(CancellationToken.None);
                        Console.WriteLine($"AuthenticateResponse: \n\tSuccess: {loginResponse.Success}, \n\tAccessToken: {loginResponse.AccessToken}, \n\tRequestId: {loginResponse.RequestId}, \n\tResponseId: {loginResponse.Id}, \n\tMessage:{loginResponse.Message}");
                    }
                    loginResponseListener.Unsubscribe();
                });
                tasks.Add(clientTask);
            }

            await Task.WhenAll(tasks).ContinueWith(action =>
            {
                stopwatch.Stop();
                Console.WriteLine($"Messages per second: {TotalMessages / stopwatch.Elapsed.TotalSeconds:N0}");
                Console.ReadKey();
            });
        }
    }
}
