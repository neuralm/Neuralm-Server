using Neuralm.Services.Common.Concurrent;
using Neuralm.Services.MessageQueue.Application.Interfaces;
using Neuralm.Services.MessageQueue.Domain;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Neuralm.Services.MessageQueue.Infrastructure
{
    /// <summary>
    /// Represents the <see cref="ServiceConnector"/> class.
    /// </summary>
    public class ServiceConnector : IServiceConnector
    {
        private readonly List<INetworkConnector> _networkConnectors;
        private readonly AsyncConcurrentQueue<object> _messageQueue;
        private static readonly Random Random = new Random();

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceConnector"/> class.
        /// </summary>
        /// <param name="networkConnector">The networkConnector.</param>
        public ServiceConnector(INetworkConnector networkConnector)
        {
            _networkConnectors = new List<INetworkConnector> { networkConnector };
            _messageQueue = new AsyncConcurrentQueue<object>();
        }

        /// <inheritdoc cref="IServiceConnector.EnqueueMessage(object)" />
        public void EnqueueMessage(object message)
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
                object message = await _messageQueue.DequeueAsync(cancellationToken);
                INetworkConnector networkConnector = GetNetworkConnector();
                if (networkConnector == null)
                    continue;
                _ = Task.Run(() => networkConnector.SendMessageAsync(message, cancellationToken), cancellationToken);
            }
        }

        /// <inheritdoc cref="IServiceConnector.AddService(INetworkConnector)" />
        public void AddService(INetworkConnector networkConnector)
        {
            _networkConnectors.Add(networkConnector);
        }

        private INetworkConnector GetNetworkConnector()
        {
            INetworkConnector networkConnector;
            int networkConnectors = _networkConnectors.Count;
            switch (networkConnectors)
            {
                case 0:
                    return null;
                case 1:
                    networkConnector = _networkConnectors[0];
                    break;
                default:
                {
                    int num = Random.Next(networkConnectors);
                    networkConnector = _networkConnectors[num];
                    break;
                }
            }

            if (networkConnector.IsConnected) 
                return networkConnector;
            _networkConnectors.Remove(networkConnector);
            return null;
        }
    }
}
