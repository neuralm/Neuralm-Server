using Microsoft.Extensions.Logging;
using Neuralm.Services.Common.Application.Interfaces;
using Neuralm.Services.Common.Exceptions;
using Neuralm.Services.Common.Messages;
using Neuralm.Services.Common.Messages.Interfaces;
using Neuralm.Services.MessageQueue.Application;
using Neuralm.Services.MessageQueue.Application.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Neuralm.Services.MessageQueue.Infrastructure
{
    /// <summary>
    /// Represents the <see cref="ServiceMessageProcessor"/> class an implementation of the <see cref="IMessageProcessor"/> interface.
    /// </summary>
    public class ServiceMessageProcessor : IServiceMessageProcessor
    {
        private readonly ILogger<ServiceMessageProcessor> _logger;
        private readonly ConcurrentDictionary<Guid, INetworkConnector> _messageToClientDictionary;
        private readonly ConcurrentDictionary<Guid, ServiceHealthCheckListener> _serviceHealthCheckListeners;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceMessageProcessor"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public ServiceMessageProcessor(ILogger<ServiceMessageProcessor> logger)
        {
            _logger = logger;
            _messageToClientDictionary = new ConcurrentDictionary<Guid, INetworkConnector>();
            _serviceHealthCheckListeners = new ConcurrentDictionary<Guid, ServiceHealthCheckListener>();
        }

        /// <inheritdoc cref="IMessageProcessor.ProcessMessageAsync(IMessage, INetworkConnector)"/>
        public Task ProcessMessageAsync(IMessage message, INetworkConnector networkConnector)
        {
            if (!(message is IResponseMessage msg))
                throw new InvalidMessageException("Only response messages are supported from a service endpoint.");

            if (message is ServiceHealthCheckResponse response)
            {
                if (_serviceHealthCheckListeners.ContainsKey(response.RequestId))
                    _serviceHealthCheckListeners[response.RequestId].OnNext(response);
                else
                    _logger.LogInformation("Received service health check response but no service health check listener found.");
                return Task.CompletedTask;
            }
                

            if (!_messageToClientDictionary.TryRemove(msg.RequestId, out INetworkConnector clientNetworkConnector))
                throw new ArgumentOutOfRangeException(nameof(message), $"Unknown message of type: {message.GetType().Name}");

            return SendMessageToClientAsync(message, clientNetworkConnector)
                .ContinueWith((task) =>
                {
                    _logger.LogInformation($"Finished Processing message: {msg.Id} from {networkConnector.EndPoint}");
                    return task;
                });
        }

        /// <inheritdoc cref="IServiceMessageProcessor.AddClientMessage(Guid, INetworkConnector)"/>
        public void AddClientMessage(Guid messageId, INetworkConnector networkConnector)
        {
            _messageToClientDictionary.TryAdd(messageId, networkConnector);
        }

        /// <inheritdoc cref="IServiceMessageProcessor.AddServiceHealthCheckMessageListener(Guid, ServiceHealthCheckListener)"/>
        public void AddServiceHealthCheckMessageListener(Guid messageId, ServiceHealthCheckListener serviceHealthCheckListener)
        {
            _serviceHealthCheckListeners.TryAdd(messageId, serviceHealthCheckListener);
        }

        /// <inheritdoc cref="IServiceMessageProcessor.RemoveServiceHealthCheckMessageListener(Guid)"/>
        public void RemoveServiceHealthCheckMessageListener(Guid messageId)
        {
            if (_serviceHealthCheckListeners.TryRemove(messageId, out ServiceHealthCheckListener serviceHealthCheckListener))
                serviceHealthCheckListener.Dispose();
        }

        private static async Task SendMessageToClientAsync(IMessage message, INetworkConnector clientNetworkConnector)
        {
            await clientNetworkConnector.SendMessageAsync(message, CancellationToken.None);
        }
    }
}
