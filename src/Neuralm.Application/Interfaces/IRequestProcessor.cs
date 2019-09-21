using System;
using System.Threading.Tasks;
using Neuralm.Application.Messages;

namespace Neuralm.Application.Interfaces
{
    /// <summary>
    /// Represents the <see cref="IRequestProcessor"/> interface.
    /// </summary>
    public interface IRequestProcessor
    {
        /// <summary>
        /// Processes requests.
        /// </summary>
        /// <param name="type">The request type.</param>
        /// <param name="request">The request.</param>
        /// <returns>Returns an awaitable <see cref="Task"/> with type parameter <see cref="IResponse"/>.</returns>
        Task<IResponse> ProcessRequest(Type type, IRequest request);
    }
}
