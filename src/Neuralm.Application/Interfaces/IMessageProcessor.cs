using System;
using System.Threading.Tasks;
using Neuralm.Application.Messages;
using Neuralm.Infrastructure.Interfaces;
using Neuralm.Utilities.Observer;

namespace Neuralm.Application.Interfaces
{
    public interface IMessageProcessor : IObservable
    {
        Task<IResponse> ProcessRequest(Type type, IRequest request, INetworkConnector networkConnector);
        Task ProcessCommand(Type type, ICommand command, INetworkConnector networkConnector);
        Task ProcessResponse(Type type, IResponse response, INetworkConnector networkConnector);
        Task ProcessEvent(Type type, IEvent @event, INetworkConnector networkConnector);
    }
}
