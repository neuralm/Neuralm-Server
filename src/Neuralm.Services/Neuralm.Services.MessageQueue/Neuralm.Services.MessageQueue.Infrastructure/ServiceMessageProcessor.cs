using Neuralm.Services.Common.Messages.Interfaces;
using Neuralm.Services.MessageQueue.Application.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Neuralm.Services.Common.Application.Interfaces;
using Neuralm.Services.Common.Exceptions;

namespace Neuralm.Services.MessageQueue.Infrastructure
{
    /// <summary>
    /// Represents the <see cref="ServiceMessageProcessor"/> class an implementation of the <see cref="IMessageProcessor"/> interface.
    /// </summary>
    public class ServiceMessageProcessor : IServiceMessageProcessor
    {
        private readonly ConcurrentDictionary<Guid, INetworkConnector> _messageToClientDictionary;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceMessageProcessor"/> class.
        /// </summary>
        public ServiceMessageProcessor()
        {
            _messageToClientDictionary = new ConcurrentDictionary<Guid, INetworkConnector>();
        }

        /// <inheritdoc cref="IMessageProcessor.ProcessMessageAsync(IMessage, INetworkConnector)"/>
        public Task ProcessMessageAsync(IMessage message, INetworkConnector networkConnector)
        {
            if (!(message is IResponseMessage msg))
                throw new InvalidMessageException("Only response messages are supported from a service endpoint.");

            if (!_messageToClientDictionary.TryRemove(msg.RequestId, out INetworkConnector clientNetworkConnector))
                throw new ArgumentOutOfRangeException(nameof(message), $"Unknown message of type: {message.GetType().Name}");

            return SendMessageToClientAsync(message, clientNetworkConnector)
                .ContinueWith((task) =>
                {
                    Console.WriteLine($"Finished Processing message: {msg.Id.ToString()} from {networkConnector.EndPoint}");
                    return task;
                });
        }

        /// <inheritdoc cref="IServiceMessageProcessor.AddClientMessage(Guid, INetworkConnector)"/>
        public void AddClientMessage(Guid messageId, INetworkConnector networkConnector)
        {
            _messageToClientDictionary.TryAdd(messageId, networkConnector);
        }
        private static async Task SendMessageToClientAsync(IMessage message, INetworkConnector clientNetworkConnector)
        {
            await clientNetworkConnector.SendMessageAsync(message, CancellationToken.None);
        }
    }
}
