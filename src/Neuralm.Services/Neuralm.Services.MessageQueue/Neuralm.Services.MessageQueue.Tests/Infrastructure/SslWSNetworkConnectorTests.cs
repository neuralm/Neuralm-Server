using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neuralm.Services.Common.Application.Interfaces;
using Neuralm.Services.Common.Application.Serializers;
using Neuralm.Services.Common.Infrastructure;
using Neuralm.Services.Common.Infrastructure.Networking;
using Neuralm.Services.Common.Messages.Interfaces;
using Neuralm.Services.Common.Patterns;
using Neuralm.Services.MessageQueue.Tests.Mocks;
using Neuralm.Services.TrainingRoomService.Messages;
using Neuralm.Services.UserService.Messages;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace Neuralm.Services.MessageQueue.Tests.Infrastructure
{
    [TestClass]
    public class SslWSNetworkConnectorTests
    {
        private const int DefaultTimeOut = 5;
        public IMessageSerializer MessageSerializer { get; set; }
        public MessageProcessorMock MessageProcessor { get; set; }
        public IMessageTypeCache MessageTypeCache { get; set; }
        public IFactory<IMessageTypeCache, IEnumerable<Type>> MessageTypeCacheFactory { get; set; }
        public string Host { get; set; }
        public ILogger<SslWSNetworkConnector> Logger { get; set; }
        public ILogger<NeuralmWSHandshakeHandler> HandshakeLogger { get; private set; }
        public X509Certificate2 X509Certificate2 { get; set; }

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
            Logger = factory.CreateLogger<SslWSNetworkConnector>();
            HandshakeLogger = factory.CreateLogger<NeuralmWSHandshakeHandler>();
            X509Certificate2 = X509Certificate2Builder.BuildSelfSignedServerCertificate(Host);
        }

        [TestMethod]
        public async Task ConnectAsync_Should_Set_IsConnected_To_True()
        {
            using CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(DefaultTimeOut * 3));
            _ = StartServer(cts.Token, 9992);
            SslWSNetworkConnector sslWSNetworkConnector = await StartClient(cts.Token, 9992);
            Assert.IsTrue(sslWSNetworkConnector.IsConnected);
            cts.Cancel();
        }

        [TestMethod]
        public async Task SendMessageAsync_Should_Invoke_MessageProcessor()
        {
            using CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(DefaultTimeOut * 3));
            _ = StartServer(cts.Token, 9993);
            SslWSNetworkConnector sslWSNetworkConnector = await StartClient(cts.Token, 9993);
            AuthenticateRequest message = new AuthenticateRequest() { CredentialTypeCode = "Name", Password = "Mario", Username = "Mario" };
            await sslWSNetworkConnector.SendMessageAsync(message, cts.Token);
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
                _ = Task.Run(async () =>
                {
                    SslWSNetworkConnector sslWSNetworkConnector = new SslWSNetworkConnector(MessageTypeCache, MessageSerializer, MessageProcessor, Logger, new NeuralmWSHandshakeHandler(HandshakeLogger), tcpClient);
                    await sslWSNetworkConnector.AuthenticateAsServerAsync(X509Certificate2, cancellationToken);
                    await sslWSNetworkConnector.StartHandshakeAsServerAsync();
                    sslWSNetworkConnector.Start();
                    Console.WriteLine("Started ssl ws network connector");
                }, cancellationToken);
            }
        }

        private async Task<SslWSNetworkConnector> StartClient(CancellationToken cancellationToken, int port)
        {
            SslWSNetworkConnector sslWSNetworkConnector = new SslWSNetworkConnector(MessageTypeCache, MessageSerializer, MessageProcessor, Logger, new NeuralmWSHandshakeHandler(HandshakeLogger),  Host, port);
            await sslWSNetworkConnector.ConnectAsync(cancellationToken);
            await sslWSNetworkConnector.AuthenticateAsClientAsync(cancellationToken);
            await sslWSNetworkConnector.StartHandshakeAsClientAsync();
            sslWSNetworkConnector.Start();
            return sslWSNetworkConnector;
        }
    }
}
