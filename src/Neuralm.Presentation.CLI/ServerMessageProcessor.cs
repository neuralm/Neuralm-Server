using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Neuralm.Application.Interfaces;
using Neuralm.Application.Messages;
using Neuralm.Application.Messages.Requests;
using Neuralm.Infrastructure.Interfaces;
using Neuralm.Utilities.Observer;

namespace Neuralm.Presentation.CLI
{
    internal class ServerMessageProcessor : IMessageProcessor
    {
        private readonly IUserService _userService;
        private readonly ITrainingRoomService _trainingRoomService;
        private readonly ConcurrentDictionary<Type, ObserverCollection> _observers;

        public ServerMessageProcessor(
            IUserService userService,
            ITrainingRoomService trainingRoomService)
        {
            _userService = userService;
            _trainingRoomService = trainingRoomService;
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

        public async Task<IResponse> ProcessRequest(Type type, IRequest request, INetworkConnector networkConnector)
        {
            object response;
            Console.WriteLine($"ProcessRequest: {request}");
            switch (request)
            {
                case AuthenticateRequest authenticateRequest:
                    response = await _userService.AuthenticateAsync(authenticateRequest);
                    break;
                case RegisterRequest registerRequest:
                    response = await _userService.RegisterAsync(registerRequest);
                    break;
                case CreateTrainingRoomRequest createTrainingRoomRequest:
                    response = await _trainingRoomService.CreateTrainingRoomAsync(createTrainingRoomRequest);
                    break;
                case GetEnabledTrainingRoomsRequest getEnabledTrainingRoomsRequest:
                    response = await _trainingRoomService.GetEnabledTrainingRoomsAsync(getEnabledTrainingRoomsRequest);
                    break;
                case StartTrainingSessionRequest startTrainingSessionRequest:
                    response = await _trainingRoomService.StartTrainingSessionAsync(startTrainingSessionRequest);
                    break;
                case EndTrainingSessionRequest endTrainingSessionRequest:
                    response = await _trainingRoomService.EndTrainingSessionAsync(endTrainingSessionRequest);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(request), $"Unknown Request message of type: {type.Name}");
            }
            Console.WriteLine($"ProcessRequest-Response: {response}");
            // ReSharper disable once PossibleInvalidCastException
            return (IResponse)response;
        }
        public Task ProcessCommand(Type type, ICommand command, INetworkConnector networkConnector)
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
        public Task ProcessResponse(Type type, IResponse response, INetworkConnector networkConnector)
        {
            return Task.Run(() =>
            {
                Console.WriteLine($"ProcessResponse: {response}");
                switch (response)
                {
                    default:
                        throw new ArgumentOutOfRangeException(nameof(response), $"Unknown Response message of type: {type.Name}");
                }
            });
        }
        public Task ProcessEvent(Type type, IEvent @event, INetworkConnector baseNetworkConnector)
        {
            return Task.Run(() =>
            {
                Console.WriteLine($"ProcessEvent: {@event}");
                switch (@event)
                {
                    default:
                        throw new ArgumentOutOfRangeException(nameof(@event), $"Unknown Event message of type: {type.Name}");
                }
            });
        }
    }
}
