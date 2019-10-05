using Neuralm.Services.Common.Concurrent;
using Neuralm.Services.Common.Messages.Interfaces;
using Neuralm.Services.MessageQueue.Application.Interfaces;
using Neuralm.Services.MessageQueue.Domain;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Neuralm.Services.MessageQueue.Infrastructure
{
    /// <summary>
    /// Represents the <see cref="ServiceConnector"/> class.
    /// </summary>
    public class ServiceConnector : IServiceConnector
    {
        private readonly ConcurrentDictionary<Guid, INetworkConnector> _networkConnectors;
        private readonly AsyncConcurrentQueue<IMessage> _messageQueue;
        private static readonly Random Random = new Random();

        /// <inheritdoc cref="IServiceConnector.Name"/>
        public string Name { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceConnector"/> class.
        /// </summary>
        /// <param name="networkConnector">The networkConnector.</param>
        /// <param name="name">The service name.</param>
        /// <param name="id">The service id.</param>
        public ServiceConnector(INetworkConnector networkConnector, string name, Guid id)
        {
            Name = name;
            _networkConnectors = new ConcurrentDictionary<Guid, INetworkConnector>();
            _networkConnectors .TryAdd(id, networkConnector);
            _messageQueue = new AsyncConcurrentQueue<IMessage>();
        }

        /// <inheritdoc cref="IServiceConnector.EnqueueMessage(IMessage)" />
        public void EnqueueMessage(IMessage message)
        {
            _messageQueue.Enqueue(message);
        }

        /// <inheritdoc cref="IServiceConnector.StartPublishingAsync(CancellationToken)" />
        public async Task StartPublishingAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await TaskEx.WaitUntil(() => _networkConnectors.Count != 0, cancellationToken: cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                IMessage message = await _messageQueue.DequeueAsync(cancellationToken);
                INetworkConnector networkConnector = null;
                while (networkConnector == null)
                {
                    networkConnector = GetNetworkConnector();
                    cancellationToken.ThrowIfCancellationRequested();
                }
                _ = Task.Run(() => networkConnector.SendMessageAsync(message, cancellationToken), cancellationToken);
            }
        }

        /// <inheritdoc cref="IServiceConnector.AddService(INetworkConnector, Guid)" />
        public void AddService(INetworkConnector networkConnector, Guid id)
        {
            _networkConnectors.TryAdd(id, networkConnector);
        }

        /// <inheritdoc cref="IServiceConnector.RemoveService(Guid)"/>
        public void RemoveService(Guid serviceId)
        {
            _networkConnectors.TryRemove(serviceId, out _);
        }

        private INetworkConnector GetNetworkConnector()
        {
            KeyValuePair<Guid, INetworkConnector> networkConnectorKeyPair;
            int networkConnectors = _networkConnectors.Count;
            switch (networkConnectors)
            {
                case 0:
                    return null;
                case 1:
                    networkConnectorKeyPair = _networkConnectors.ElementAt(0);
                    break;
                default:
                {
                    int num = Random.Next(networkConnectors);
                    networkConnectorKeyPair = _networkConnectors.ElementAt(num);
                    break;
                }
            }

            if (networkConnectorKeyPair.Value.IsConnected) 
                return networkConnectorKeyPair.Value;
            RemoveService(networkConnectorKeyPair.Key);
            return null;
        }
    }
}
