using Neuralm.Application.Interfaces;
using Neuralm.Application.Messages;
using Neuralm.Utilities.Observer;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Neuralm.Client
{
    /// <summary>
    /// Represents the <see cref="ClientMessageProcessor"/> class, an implementation of the <see cref="IMessageProcessor"/> interface.
    /// </summary>
    internal class ClientMessageProcessor : IMessageProcessor
    {
        private readonly ConcurrentDictionary<Type, ObserverCollection> _observers;

        /// <summary>
        /// Initializes an instance of the <see cref="ClientMessageProcessor"/> class.
        /// </summary>
        public ClientMessageProcessor()
        {
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

        /// <inheritdoc cref="IRequestProcessor.ProcessRequest(Type, IRequest)"/>
        public Task<IResponse> ProcessRequest(Type type, IRequest request)
        {
            return Task.Run(() =>
            {
                Console.WriteLine("Received IRequest message");
                // ...
                return default(Task<IResponse>);
            });
        }

        /// <inheritdoc cref="IMessageProcessor.ProcessCommand(Type, ICommand)"/>
        public Task ProcessCommand(Type type, ICommand command)
        {
            return Task.Run(() =>
            {
                Console.WriteLine("Received ICommand message");
                // ...
            });
        }

        /// <inheritdoc cref="IMessageProcessor.ProcessResponse(Type, IResponse)"/>
        public Task ProcessResponse(Type type, IResponse response)
        {
            return Task.Run(() =>
            {
                Console.WriteLine("Received IResponse message");
                if (_observers.ContainsKey(response.GetType()))
                    _observers[response.GetType()].OnNextAll(response);
                else
                    Console.WriteLine($"Message Response of type: {type.FullName} not found in listeners.");
            });
        }

        /// <inheritdoc cref="IMessageProcessor.ProcessEvent(Type, IEvent)"/>
        public Task ProcessEvent(Type type, IEvent @event)
        {
            return Task.Run(() =>
            {
                Console.WriteLine("Received IEvent message");
                if (_observers.ContainsKey(@event.GetType()))
                    _observers[@event.GetType()].OnNextAll(@event);
                else
                    Console.WriteLine($"Message Event of type: {type.FullName} not found in listeners.");
            });
        }
    }
}
