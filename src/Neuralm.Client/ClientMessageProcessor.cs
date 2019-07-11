using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Neuralm.Application.Interfaces;
using Neuralm.Application.Messages;
using Neuralm.Infrastructure.Interfaces;
using Neuralm.Utilities.Observer;

namespace Neuralm.Client
{
    internal class ClientMessageProcessor : IMessageProcessor
    {
        private readonly ConcurrentDictionary<Type, ObserverCollection> _observers;

        public ClientMessageProcessor()
        {
            _observers = new ConcurrentDictionary<Type, ObserverCollection>();
        }

        public IDisposable Subscribe(Type type, IObserver observer)
        {
            if (_observers.ContainsKey(type))
                _observers[type].Add(observer);
            else
                _observers.TryAdd(type, new ObserverCollection(observer));

            return new ObserverUnsubscriber(_observers[type], observer);
        }

        public Task<IResponse> ProcessRequest(Type type, IRequest request, INetworkConnector networkConnector)
        {
            return Task.Run(() =>
            {
                Console.WriteLine("Received IRequest message");
                // ...
                return default(Task<IResponse>);
            });
        }

        public Task ProcessCommand(Type type, ICommand command, INetworkConnector networkConnector)
        {
            return Task.Run(() =>
            {
                Console.WriteLine("Received ICommand message");
                // ...
            });
        }

        public Task ProcessResponse(Type type, IResponse response, INetworkConnector networkConnector)
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

        public Task ProcessEvent(Type type, IEvent @event, INetworkConnector networkConnector)
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
