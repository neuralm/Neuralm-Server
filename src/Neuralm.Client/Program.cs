using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Neuralm.Application.Interfaces;
using Neuralm.Application.Messages;
using Neuralm.Application.Messages.Requests;
using Neuralm.Application.Messages.Responses;
using Neuralm.Domain.Entities.NEAT;
using Neuralm.Infrastructure.Interfaces;
using Neuralm.Infrastructure.MessageSerializers;
using Neuralm.Infrastructure.Networking;

namespace Neuralm.Client
{
    internal static class Program
    {
        private const int Port = 9999;
        private const string Host = "localhost";
        private const int MessageCount = 1;
        private const int ClientCount = 1;
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
                    SslTcpNetworkConnector networkConnector = new SslTcpNetworkConnector(messageSerializer, messageProcessor, Host, Port);
                    await networkConnector.ConnectAsync(CancellationToken.None);
                    await networkConnector.AuthenticateAsClient(CancellationToken.None);
                    networkConnector.Start();

                    MessageListener<AuthenticateResponse> loginResponseListener = new MessageListener<AuthenticateResponse>();
                    MessageListener<RegisterResponse> registerMessageListener = new MessageListener<RegisterResponse>();
                    MessageListener<CreateTrainingRoomResponse> createTrainingRoomResponseListener = new MessageListener<CreateTrainingRoomResponse>();
                    MessageListener<GetEnabledTrainingRoomsResponse> getEnabledTrainingRoomsResponseListener = new MessageListener<GetEnabledTrainingRoomsResponse>();
                    MessageListener<StartTrainingSessionResponse> startTrainingSessionResponseListener = new MessageListener<StartTrainingSessionResponse>();
                    MessageListener<GetOrganismsResponse> getOrganismsResponseListener = new MessageListener<GetOrganismsResponse>();
                    loginResponseListener.Subscribe(messageProcessor);
                    createTrainingRoomResponseListener.Subscribe(messageProcessor);
                    registerMessageListener.Subscribe(messageProcessor);
                    getEnabledTrainingRoomsResponseListener.Subscribe(messageProcessor);
                    startTrainingSessionResponseListener.Subscribe(messageProcessor);
                    getOrganismsResponseListener.Subscribe(messageProcessor);
                    for (int i = 0; i < MessageCount; i++)
                    {
                        Guid name = Guid.NewGuid();
                        RegisterRequest registerRequest = new RegisterRequest(name.ToString(), "password", "Name");
                        await networkConnector.SendMessageAsync(registerRequest, CancellationToken.None);
                        RegisterResponse registerResponse = await registerMessageListener.ReceiveMessageAsync(CancellationToken.None);
                        Console.WriteLine($"RegisterResponse: \n\tSuccess: {registerResponse.Success}, \n\tRequestId: {registerResponse.RequestId}, \n\tResponseId: {registerResponse.Id}, \n\tMessage:{registerResponse.Message}");

                        AuthenticateRequest loginRequest = new AuthenticateRequest(name.ToString(), "password", "Name");
                        await networkConnector.SendMessageAsync(loginRequest, CancellationToken.None);
                        AuthenticateResponse loginResponse = await loginResponseListener.ReceiveMessageAsync(CancellationToken.None);
                        Console.WriteLine($"AuthenticateResponse: \n\tSuccess: {loginResponse.Success}, \n\tAccessToken: {loginResponse.AccessToken}, \n\tRequestId: {loginResponse.RequestId}, \n\tResponseId: {loginResponse.Id}, \n\tMessage:{loginResponse.Message}");

                        TrainingRoomSettings trainingRoomSettings = new TrainingRoomSettings(50, 2, 1, 1, 1, 0.4, 3, 0.05, 0.03, 0.75, 0.001, 1, 0.8, 0.1, 0.5, 0.25, 0);
                        CreateTrainingRoomRequest createTrainingRoomRequest = new CreateTrainingRoomRequest(loginResponse.UserId, Guid.NewGuid().ToString(), trainingRoomSettings);
                        await networkConnector.SendMessageAsync(createTrainingRoomRequest, CancellationToken.None);
                        CreateTrainingRoomResponse createTrainingRoomResponse = await createTrainingRoomResponseListener.ReceiveMessageAsync(CancellationToken.None);
                        Console.WriteLine($"CreateTrainingRoomResponse: \n\tId: {createTrainingRoomResponse.Id}\n\tRequestId: {createTrainingRoomResponse.RequestId}\n\tDateTime: {createTrainingRoomResponse.DateTime}\n\tMessage: {createTrainingRoomResponse.Message}\n\tSuccess: {createTrainingRoomResponse.Success}\n\tTrainingRoomId: {createTrainingRoomResponse.TrainingRoomId}");

                        StartTrainingSessionRequest startTrainingSessionRequest = new StartTrainingSessionRequest(loginResponse.UserId, createTrainingRoomResponse.TrainingRoomId);
                        await networkConnector.SendMessageAsync(startTrainingSessionRequest, CancellationToken.None);
                        StartTrainingSessionResponse startTrainingSessionResponse = await startTrainingSessionResponseListener.ReceiveMessageAsync(CancellationToken.None);
                        Console.WriteLine($"StartTrainingSessionResponse: \n\tId: {startTrainingSessionResponse.Id}\n\tRequestId: {startTrainingSessionResponse.RequestId}\n\tDateTime: {startTrainingSessionResponse.DateTime}\n\tMessage: {startTrainingSessionResponse.Message}\n\tSuccess: {startTrainingSessionResponse.Success}\n\tTrainingSessionId: {startTrainingSessionResponse.TrainingSession.Id}");

                        GetOrganismsRequest getOrganismsRequest = new GetOrganismsRequest(startTrainingSessionResponse.TrainingSession.Id, 10);
                        await networkConnector.SendMessageAsync(getOrganismsRequest, CancellationToken.None);
                        GetOrganismsResponse getOrganismsResponse = await getOrganismsResponseListener.ReceiveMessageAsync(CancellationToken.None);
                        Console.WriteLine($"GetOrganismsResponse: \n\tId: {getOrganismsResponse.Id}\n\tRequestId: {getOrganismsResponse.RequestId}\n\tDateTime: {getOrganismsResponse.DateTime}\n\tMessage: {getOrganismsResponse.Message}\n\tSuccess: {getOrganismsResponse.Success}\n\tOrganismsCount: {getOrganismsResponse.Organisms.Count()}\n\tConnectionGenesCount: {getOrganismsResponse.Organisms.Sum(o => o.Brain.ConnectionGenes.Count)}");

                        GetEnabledTrainingRoomsRequest getEnabledTrainingRoomsRequest = new GetEnabledTrainingRoomsRequest();
                        await networkConnector.SendMessageAsync(getEnabledTrainingRoomsRequest, CancellationToken.None);
                        GetEnabledTrainingRoomsResponse getEnabledTrainingRoomsResponse = await getEnabledTrainingRoomsResponseListener.ReceiveMessageAsync(CancellationToken.None);
                        Console.WriteLine($"GetEnabledTrainingRoomsResponse: \n\tId: {getEnabledTrainingRoomsResponse.Id}\n\tRequestId: {getEnabledTrainingRoomsResponse.RequestId}\n\tDateTime: {getEnabledTrainingRoomsResponse.DateTime}\n\tMessage: {getEnabledTrainingRoomsResponse.Message}\n\tSuccess: {getEnabledTrainingRoomsResponse.Success}");


                        //foreach (TrainingRoomDto trainingRoomDto in getEnabledTrainingRoomsResponse.TrainingRooms)
                        //    Console.WriteLine($"TrainingRoom:\n\tId: {trainingRoomDto.Id}\n\tName: {trainingRoomDto.Name}\n\tOwner: {trainingRoomDto.Owner.Username}");
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
