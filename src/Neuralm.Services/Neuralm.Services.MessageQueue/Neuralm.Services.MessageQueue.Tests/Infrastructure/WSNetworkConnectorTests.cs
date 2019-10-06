using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neuralm.Services.Common.Messages.Interfaces;
using Neuralm.Services.MessageQueue.Application.Interfaces;
using Neuralm.Services.MessageQueue.Application.Serializers;
using Neuralm.Services.MessageQueue.Infrastructure.Networking;
using Neuralm.Services.MessageQueue.Tests.Mocks;
using Neuralm.Services.UserService.Messages;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Neuralm.Services.MessageQueue.Tests.Infrastructure
{
    [TestClass]
    public class WSNetworkConnectorTests
    {
        private const int DefaultTimeOut = 5;
        public IMessageSerializer MessageSerializer { get; set; }
        public MessageProcessorMock MessageProcessor { get; set; }
        public string Host { get; set; }

        [TestInitialize]
        public void Setup()
        {
            Host = "localhost";
            MessageSerializer = new JsonMessageSerializer();
            MessageProcessor = new MessageProcessorMock(MessageSerializer);
        }

        [TestMethod]
        public async Task ConnectAsync_Should_Set_IsConnected_To_True()
        {
            CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(DefaultTimeOut * 3));
            _ = StartServer(cts.Token, 9986);

            WSNetworkConnector wsNetworkConnector = await StartClient(cts.Token, 9986);
            Assert.IsTrue(wsNetworkConnector.IsConnected);
            cts.Cancel();
        }

        [TestMethod]
        public async Task SendMessageAsync_Should_Invoke_MessageProcessor()
        {
            CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(DefaultTimeOut * 3));
            _ = StartServer(cts.Token, 9987);

            WSNetworkConnector wsNetworkConnector = await StartClient(cts.Token, 9987);
            AuthenticateRequest message = new AuthenticateRequest() { CredentialTypeCode = "Name", Password = "Mario", Username = "Mario" };

            await wsNetworkConnector.SendMessageAsync(message, cts.Token);
            IMessage messageInMessageProcessor = await MessageProcessor.GetMessageAsync(cts.Token);
            Assert.AreEqual(message.Id, messageInMessageProcessor.Id);
            cts.Cancel();
        }

        private async Task StartServer(CancellationToken cancellationToken, int port)
        {
            Console.WriteLine("Starting listener...");
            TcpListener tcpListener = new TcpListener(IPAddress.Loopback, port);
            tcpListener.Start();
            Console.WriteLine("Stared listener!");
            while (!cancellationToken.IsCancellationRequested)
            {
                Console.WriteLine("Awaiting new connection...");
                TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();
                Console.WriteLine("Accepted connection!");
                _ = Task.Run(async () =>
                {
                    WSNetworkConnector networkConnector = new WSNetworkConnector(MessageSerializer, MessageProcessor, tcpClient);
                    await networkConnector.StartHandshakeAsServerAsync();
                    networkConnector.Start();
                    Console.WriteLine("Started websocket network connector");
                }, cancellationToken);
            }
        }

        private async Task<WSNetworkConnector> StartClient(CancellationToken cancellationToken, int port)
        {
            WSNetworkConnector wsNetworkConnector = new WSNetworkConnector(MessageSerializer, MessageProcessor, Host, port);
            await wsNetworkConnector.ConnectAsync(cancellationToken);
            await wsNetworkConnector.StartHandshakeAsClientAsync();
            wsNetworkConnector.Start();
            return wsNetworkConnector;
        }
    }
}
