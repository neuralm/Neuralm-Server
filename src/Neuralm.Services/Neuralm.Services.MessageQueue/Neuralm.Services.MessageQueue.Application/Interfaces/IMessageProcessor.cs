using Neuralm.Services.Common.Messages.Interfaces;
using System;
using System.Threading.Tasks;

namespace Neuralm.Services.MessageQueue.Application.Interfaces
{
    /// <summary>
    /// Represents the <see cref="IMessageProcessor"/> interface.
    /// </summary>
    public interface IMessageProcessor
    {
        /// <summary>
        /// Processes a request asynchronously.
        /// </summary>
        /// <param name="type">The message type.</param>
        /// <param name="requestMessage">The request message.</param>
        /// <returns></returns>
        Task<IResponse> ProcessRequestAsync(Type type, IRequest requestMessage);

        /// <summary>
        /// Processes a command asynchronously.
        /// </summary>
        /// <param name="type">The message type.</param>
        /// <param name="commandMessage">The command message.</param>
        /// <returns></returns>
        Task ProcessCommandAsync(Type type, ICommand commandMessage);
    }
}
