using Neuralm.Services.Common.Messages.Interfaces;
using Neuralm.Services.Common.Observer;
using Neuralm.Services.MessageQueue.Application.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Threading.Tasks;
using Neuralm.Services.MessageQueue.Application;

namespace Neuralm.Services.MessageQueue.NeuralmMQ
{
    /// <summary>
    /// Represents the <see cref="NeuralmMessageProcessor"/> class an implementation of the <see cref="IMessageProcessor"/> interface.
    /// </summary>
    internal class NeuralmMessageProcessor : IMessageProcessor
    {
        private readonly MessageToServiceMapper _messageToServiceMapper;
        private readonly ConcurrentDictionary<Type, ObserverCollection> _observers;

        /// <summary>
        /// Initializes an instance of the <see cref="NeuralmMessageProcessor"/> class.
        /// </summary>
        /// <param name="messageToServiceMapper">The message to service mapper.</param>
        public NeuralmMessageProcessor(MessageToServiceMapper messageToServiceMapper, IRegistryService serviceRegistry)
        {
            _messageToServiceMapper = messageToServiceMapper;
            _observers = new ConcurrentDictionary<Type, ObserverCollection>();
        }

        /// <inheritdoc cref="IObservable.Subscribe(Type, IObserver)"/>
        public IDisposable Subscribe(Type type, IObserver observer)
        {
            if (_observers.ContainsKey(type))
                _observers[type].Add(observer);
            else
                _observers.TryAdd(type, new ObserverCollection(observer));

            return new ObserverUnsubscriber(_observers[type], observer);
        }

        /// <inheritdoc cref="IMessageProcessor.ProcessRequestAsync(Type, IRequest)"/>
        public async Task<IResponse> ProcessRequestAsync(Type type, IRequest request)
        {
            object response;
            Console.WriteLine($"ProcessRequest: {request}");
            if (_messageToServiceMapper.MessageToServiceMap.TryGetValue(type, out IServiceConnector serviceConnector))
            {
                serviceConnector.EnqueueMessage(request);
            }
            else
                throw new ArgumentOutOfRangeException(nameof(request), $"Unknown Request message of type: {type.Name}");

            Console.WriteLine($"ProcessRequest-Response: {response}");
            // ReSharper disable once PossibleInvalidCastException
            return (IResponse)response;
        }

        /// <inheritdoc cref="IMessageProcessor.ProcessCommandAsync(Type, ICommand)"/>
        public Task ProcessCommandAsync(Type type, ICommand command)
        {
            return Task.Run(() =>
            {
                Console.WriteLine($"ProcessCommand: {command}");
                switch (command)
                {
                    default:
                        throw new ArgumentOutOfRangeException(nameof(command), $"Unknown Command message of type: {type.Name}");
                }
            });
        }
    }
}
