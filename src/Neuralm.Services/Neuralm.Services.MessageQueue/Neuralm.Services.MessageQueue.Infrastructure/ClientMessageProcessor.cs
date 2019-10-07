using Microsoft.Extensions.Options;
using Neuralm.Services.Common.Messages.Interfaces;
using Neuralm.Services.MessageQueue.Application.Configurations;
using Neuralm.Services.MessageQueue.Application.Interfaces;
using Neuralm.Services.MessageQueue.Infrastructure.Networking;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Neuralm.Services.MessageQueue.Infrastructure
{
    /// <summary>
    /// Represents the <see cref="ClientMessageProcessor"/> class an implementation of the <see cref="IMessageProcessor"/> interface.
    /// </summary>
    public class ClientMessageProcessor : IClientMessageProcessor
    {
        private readonly IMessageSerializer _messageSerializer;
        private readonly IServiceMessageProcessor _serviceMessageProcessor;
        private readonly IMessageToServiceMapper _messageToServiceMapper;
        private readonly MessageQueueConfiguration _messageQueueConfiguration;
        private readonly TcpListener _tcpListener;

        /// <summary>
        /// Initializes an instance of the <see cref="ClientMessageProcessor"/> class.
        /// </summary>
        /// <param name="messageToServiceMapper">The message to service mapper.</param>
        /// <param name="messageSerializer">The message serializer.</param>
        /// <param name="messageQueueConfigurationOptions">The message queue configuration options.</param>
        /// <param name="serviceMessageProcessor">The service message processor.</param>
        public ClientMessageProcessor(
            IMessageToServiceMapper messageToServiceMapper, 
            IMessageSerializer messageSerializer,
            IOptions<MessageQueueConfiguration> messageQueueConfigurationOptions,
            IServiceMessageProcessor serviceMessageProcessor)
        {
            _messageToServiceMapper = messageToServiceMapper;
            _messageSerializer = messageSerializer;
            _serviceMessageProcessor = serviceMessageProcessor;
            _messageQueueConfiguration = messageQueueConfigurationOptions.Value;
            _tcpListener = new TcpListener(IPAddress.Any, _messageQueueConfiguration.Port);
        }

        /// <inheritdoc cref="IClientMessageProcessor.StartAsync(CancellationToken)"/>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _tcpListener.Start();
            while (!cancellationToken.IsCancellationRequested)
            {
                TcpClient tcpClient = await _tcpListener.AcceptTcpClientAsync();
                _ = Task.Run(async () =>
                {
                    //SslTcpNetworkConnector networkConnector = new SslTcpNetworkConnector(_messageSerializer, this, tcpClient);
                    WSNetworkConnector networkConnector = new WSNetworkConnector(_messageSerializer, this, tcpClient);
                    //await networkConnector.AuthenticateAsServer(_messageQueueConfiguration.Certificate, CancellationToken.None);
                    await networkConnector.StartHandshakeAsServerAsync();
                    networkConnector.Start();
                }, cancellationToken);
            }
        }

        /// <inheritdoc cref="IMessageProcessor.ProcessMessageAsync(IMessage, INetworkConnector)"/>
        public Task ProcessMessageAsync(IMessage message, INetworkConnector networkConnector)
        {
            return Task.Run(() =>
            {
                Console.WriteLine($"Started Processing message: {message} from {networkConnector.EndPoint}");
                if (_messageToServiceMapper.MessageToServiceMap.TryGetValue(message.GetType(), out IServiceConnector serviceConnector))
                {
                    _serviceMessageProcessor.AddClientMessage(message.Id, networkConnector);
                    serviceConnector.EnqueueMessage(message);
                }
                else
                    throw new ArgumentOutOfRangeException(nameof(message), "Unknown message of type: {message.GetType().Name}");
            });
        }
    }
}
