using System;
using System.Threading.Tasks;
using Neuralm.Application.Messages;
using Neuralm.Infrastructure.Interfaces;
using Neuralm.Utilities.Observer;

namespace Neuralm.Application.Interfaces
{
    /// <summary>
    /// Represents the <see cref="IMessageProcessor"/> interface.
    /// </summary>
    public interface IMessageProcessor : IObservable
    {
        /// <summary>
        /// Processes requests.
        /// </summary>
        /// <param name="type">The request type.</param>
        /// <param name="request">The request.</param>
        /// <param name="networkConnector">The network connector.</param>
        /// <returns>Returns an awaitable <see cref="Task"/> with type parameter <see cref="IResponse"/>.</returns>
        Task<IResponse> ProcessRequest(Type type, IRequest request, INetworkConnector networkConnector);

        /// <summary>
        /// Processes commands.
        /// </summary>
        /// <param name="type">The command type.</param>
        /// <param name="command">The command.</param>
        /// <param name="networkConnector">The network connector.</param>
        /// <returns>Returns an awaitable <see cref="Task"/>.</returns>
        Task ProcessCommand(Type type, ICommand command, INetworkConnector networkConnector);

        /// <summary>
        /// Processes responses.
        /// </summary>
        /// <param name="type">The response type.</param>
        /// <param name="response">The response.</param>
        /// <param name="networkConnector">The network connector.</param>
        /// <returns>Returns an awaitable <see cref="Task"/>.</returns>
        Task ProcessResponse(Type type, IResponse response, INetworkConnector networkConnector);

        /// <summary>
        /// Processes events.
        /// </summary>
        /// <param name="type">The response type.</param>
        /// <param name="event">The event.</param>
        /// <param name="networkConnector">The network connector.</param>
        /// <returns>Returns an awaitable <see cref="Task"/>.</returns>
        Task ProcessEvent(Type type, IEvent @event, INetworkConnector networkConnector);
    }
}
