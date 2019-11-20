using Neuralm.Services.Common.Messages.Interfaces;
using Neuralm.Services.MessageQueue.Application.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Neuralm.Services.Common.Application.Interfaces;

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
        public async Task ProcessMessageAsync(IMessage message, INetworkConnector networkConnector)
        {
            if (_messageToClientDictionary.TryRemove(message.Id, out INetworkConnector clientNetworkConnector))
                await clientNetworkConnector.SendMessageAsync(message, CancellationToken.None);
            else
                throw new ArgumentOutOfRangeException(nameof(message), $"Unknown message of type: {message.GetType().Name}");
            Console.WriteLine($"Finished Processing message: {message.Id.ToString()} from {networkConnector.EndPoint}");
        }

        /// <inheritdoc cref="IServiceMessageProcessor.AddClientMessage(Guid, INetworkConnector)"/>
        public void AddClientMessage(Guid messageId, INetworkConnector networkConnector)
        {
            _messageToClientDictionary.TryAdd(messageId, networkConnector);
        }
    }
}
