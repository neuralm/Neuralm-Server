using System.Threading;
using System.Threading.Tasks;
using Neuralm.Infrastructure.Interfaces;

namespace Neuralm.Application.Interfaces
{
    /// <summary>
    /// Represents the <see cref="IRestEndPoint"/> interface.
    /// </summary>
    public interface IRestEndPoint
    {
        /// <summary>
        /// Starts the rest end point asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="requestProcessor">The request processor.</param>
        /// <param name="messageSerializer">The message serializer.</param>
        /// <returns>Returns an awaitable <see cref="Task"/>.</returns>
        Task StartAsync(CancellationToken cancellationToken, IRequestProcessor requestProcessor, IMessageSerializer messageSerializer);

        /// <summary>
        /// Stops the rest end point asynchronously.
        /// </summary>
        /// <returns>Returns an awaitable <see cref="Task"/>.</returns>
        Task StopAsync();
    }
}
