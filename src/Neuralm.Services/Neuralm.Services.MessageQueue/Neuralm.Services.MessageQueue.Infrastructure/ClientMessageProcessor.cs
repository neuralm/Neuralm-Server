using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Neuralm.Services.Common.Application.Interfaces;
using Neuralm.Services.Common.Exceptions;
using Neuralm.Services.Common.Infrastructure.Networking;
using Neuralm.Services.Common.Messages.Interfaces;
using Neuralm.Services.MessageQueue.Application.Configurations;
using Neuralm.Services.MessageQueue.Application.Interfaces;
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
        private readonly MessageQueueConfiguration _messageQueueConfiguration;
        private readonly IServiceMessageProcessor _serviceMessageProcessor;
        private readonly IClientMessageTypeCache _messageTypeCache;
        private readonly ILogger<ClientMessageProcessor> _clientMessageProcessorLogger;
        private readonly ILogger<SslWSNetworkConnector> _sslWSNetworkConnectorLogger;
        private readonly ILogger<NeuralmWSHandshakeHandler> _wsHandshakeLogger;
        private readonly IMessageToServiceMapper _messageToServiceMapper;
        private readonly TcpListener _tcpListener;

        /// <summary>
        /// Initializes an instance of the <see cref="ClientMessageProcessor"/> class.
        /// </summary>
        /// <param name="messageToServiceMapper">The message to service mapper.</param>
        /// <param name="messageSerializer">The message serializer.</param>
        /// <param name="messageQueueConfigurationOptions">The message queue configuration options.</param>
        /// <param name="serviceMessageProcessor">The service message processor.</param>
        /// <param name="messageTypeCache">The message type cache.</param>
        /// <param name="clientMessageProcessorLogger">The client message processor logger.</param>
        /// <param name="sslWSNetworkConnectorLogger">The ssl websocket network connector logger.</param>
        /// <param name="wsHandshakeLogger">The websocket handshake logger.</param>
        public ClientMessageProcessor(
            IMessageToServiceMapper messageToServiceMapper, 
            IMessageSerializer messageSerializer,
            IOptions<MessageQueueConfiguration> messageQueueConfigurationOptions,
            IServiceMessageProcessor serviceMessageProcessor,
            IClientMessageTypeCache messageTypeCache,
            ILogger<ClientMessageProcessor> clientMessageProcessorLogger,
            ILogger<SslWSNetworkConnector> sslWSNetworkConnectorLogger,
            ILogger<NeuralmWSHandshakeHandler> wsHandshakeLogger)
        {
            _messageToServiceMapper = messageToServiceMapper;
            _messageSerializer = messageSerializer;
            _messageQueueConfiguration = messageQueueConfigurationOptions.Value;
            _serviceMessageProcessor = serviceMessageProcessor;
            _messageTypeCache = messageTypeCache;
            _clientMessageProcessorLogger = clientMessageProcessorLogger;
            _sslWSNetworkConnectorLogger = sslWSNetworkConnectorLogger;
            _wsHandshakeLogger = wsHandshakeLogger;
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
                    SslWSNetworkConnector networkConnector = new SslWSNetworkConnector(
                        _messageTypeCache, 
                        _messageSerializer, 
                        this, 
                        _sslWSNetworkConnectorLogger, 
                        new NeuralmWSHandshakeHandler(_wsHandshakeLogger), 
                        tcpClient);
                    await networkConnector.AuthenticateAsServerAsync(_messageQueueConfiguration.Certificate, cancellationToken);
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
                _clientMessageProcessorLogger.LogInformation($"Started Processing message: {message} from {networkConnector.EndPoint}");
                // TODO: Detect RateLimiting here

                if (_messageToServiceMapper.MessageToServiceMap.TryGetValue(message.GetType(), out IServiceConnector serviceConnector))
                {
                    _serviceMessageProcessor.AddClientMessage(message.Id, networkConnector);
                    serviceConnector.EnqueueMessage(message);
                }
                else
                    throw new InvalidMessageException($"Unknown message of type: {message.GetType().Name}");
            });
        }
    }
}
