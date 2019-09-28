using System;
using System.Threading.Tasks;
using Neuralm.Application.Messages;
using Neuralm.Utilities.Observer;

namespace Neuralm.Application.Interfaces
{
    /// <summary>
    /// Represents the <see cref="IMessageProcessor"/> interface.
    /// </summary>
    public interface IMessageProcessor : IObservable, IRequestProcessor
    {
        /// <summary>
        /// Processes commands.
        /// </summary>
        /// <param name="type">The command type.</param>
        /// <param name="command">The command.</param>
        /// <returns>Returns an awaitable <see cref="Task"/>.</returns>
        Task ProcessCommand(Type type, ICommand command);

        /// <summary>
        /// Processes responses.
        /// </summary>
        /// <param name="type">The response type.</param>
        /// <param name="response">The response.</param>
        /// <returns>Returns an awaitable <see cref="Task"/>.</returns>
        Task ProcessResponse(Type type, IResponse response);

        /// <summary>
        /// Processes events.
        /// </summary>
        /// <param name="type">The response type.</param>
        /// <param name="event">The event.</param>
        /// <returns>Returns an awaitable <see cref="Task"/>.</returns>
        Task ProcessEvent(Type type, IEvent @event);
    }
}
