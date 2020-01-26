using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neuralm.Services.Common.Application.Interfaces;
using Neuralm.Services.Common.Application.Serializers;
using Neuralm.Services.Common.Infrastructure;
using Neuralm.Services.Common.Infrastructure.Networking;
using Neuralm.Services.Common.Messages.Interfaces;
using Neuralm.Services.MessageQueue.Tests.Mocks;
using Neuralm.Services.TrainingRoomService.Messages;
using Neuralm.Services.UserService.Messages;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Neuralm.Services.MessageQueue.Tests.Infrastructure
{
    [TestClass]
    public class TcpNetworkConnectorTests
    {
        private const int DefaultTimeOut = 5;
        public IMessageSerializer MessageSerializer { get; set; }
        public MessageProcessorMock MessageProcessor { get; set; }
        public IMessageTypeCache MessageTypeCache { get; set; }
        public IFactory<IMessageTypeCache, IEnumerable<Type>> MessageTypeCacheFactory { get; set; }
        public string Host { get; set; }
        public ILogger<TcpNetworkConnector> Logger { get; set; }

        [TestInitialize]
        public void Setup()
        {
            Host = "localhost";
            MessageSerializer = new JsonMessageSerializer();
            MessageProcessor = new MessageProcessorMock(MessageSerializer);
            MessageTypeCacheFactory = new MessageTypeCacheFactory();
            List<Type> types = new List<Type>
            {
                typeof(RegisterRequest), typeof(CreateTrainingRoomRequest)
            };
            MessageTypeCache = MessageTypeCacheFactory.Create(types);
            IServiceProvider serviceProvider = new ServiceCollection().AddLogging(builder => builder.AddConsole()).BuildServiceProvider();
            ILoggerFactory factory = serviceProvider.GetService<ILoggerFactory>();
            Logger = factory.CreateLogger<TcpNetworkConnector>();
        }

        [TestMethod]
        public async Task ConnectAsync_Should_Set_IsConnected_To_True()
        {
            using CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(DefaultTimeOut * 3));
            _ = StartServer(cts.Token, 9989);
            TcpNetworkConnector tcpNetworkConnector = await StartClient(cts.Token, 9989);
            Assert.IsTrue(tcpNetworkConnector.IsConnected);
            cts.Cancel();
        }

        [TestMethod]
        public async Task SendMessageAsync_Should_Invoke_MessageProcessor()
        {
            using CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(DefaultTimeOut * 3));
            _ = StartServer(cts.Token, 9988);
            TcpNetworkConnector tcpNetworkConnector = await StartClient(cts.Token, 9988);
            AuthenticateRequest message = new AuthenticateRequest() { CredentialTypeCode = "Name", Password = "Mario", Username = "Mario" };
            await tcpNetworkConnector.SendMessageAsync(message, cts.Token);
            IMessage messageInMessageProcessor = await MessageProcessor.GetMessageAsync(cts.Token);
            Assert.AreEqual(message.Id, messageInMessageProcessor?.Id);
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
                _ = Task.Run(() =>
                {
                    TcpNetworkConnector networkConnector = new TcpNetworkConnector(MessageTypeCache, MessageSerializer, MessageProcessor, Logger, tcpClient);
                    networkConnector.Start();
                    Console.WriteLine("Started tcp network connector");
                }, cancellationToken);
            }
        }


        private async Task<TcpNetworkConnector> StartClient(CancellationToken cancellationToken, int port)
        {
            TcpNetworkConnector tcpNetworkConnector = new TcpNetworkConnector(MessageTypeCache, MessageSerializer, MessageProcessor, Logger, Host, port);
            await tcpNetworkConnector.ConnectAsync(cancellationToken);
            tcpNetworkConnector.Start();
            return tcpNetworkConnector;
        }
    }
}
