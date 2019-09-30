using Microsoft.Extensions.Options;
using Neuralm.Services.MessageQueue.Application.Configurations;
using Neuralm.Services.MessageQueue.Application.Interfaces;
using Neuralm.Services.MessageQueue.Infrastructure.Networking;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Neuralm.Services.MessageQueue.Infrastructure.Messaging
{
    /// <summary>
    /// Represents the <see cref="MessageQueue"/> class.
    /// </summary>
    public class MessageQueue : IMessageQueue
    {
        private readonly MessageQueueConfiguration _messageQueueConfiguration;
        private readonly TcpListener _tcpListener;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageQueue"/> class.
        /// </summary>
        public MessageQueue(IOptions<MessageQueueConfiguration> messageQueueConfigurationOptions)
        {
            _messageQueueConfiguration = messageQueueConfigurationOptions.Value;
            _tcpListener = new TcpListener(IPAddress.Any, _messageQueueConfiguration.Port);
        }

        /// <inheritdoc cref="IMessageQueue.StartAsync(CancellationToken, IMessageProcessor, IMessageSerializer)"/>
        public async Task StartAsync(CancellationToken cancellationToken, IMessageProcessor messageProcessor, IMessageSerializer messageSerializer)
        {
            _tcpListener.Start();
            while (!cancellationToken.IsCancellationRequested)
            {
                TcpClient tcpClient = await _tcpListener.AcceptTcpClientAsync();
                _ = Task.Run(async () =>
                {
                    SslTcpNetworkConnector networkConnector = new SslTcpNetworkConnector(messageSerializer, messageProcessor, tcpClient);
                    await networkConnector.AuthenticateAsServer(_messageQueueConfiguration.Certificate, CancellationToken.None);
                    networkConnector.Start();
                }, cancellationToken);
            }
        }

        /// <inheritdoc cref="IMessageQueue.EnqueueAsync()"/>
        public Task EnqueueAsync()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="IMessageQueue.DequeueAsync()"/>
        public Task DequeueAsync()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="IMessageQueue.Stop()"/>
        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
