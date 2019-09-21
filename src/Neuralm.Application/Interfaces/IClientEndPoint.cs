using System.Threading;
using System.Threading.Tasks;
using Neuralm.Infrastructure.Interfaces;

namespace Neuralm.Application.Interfaces
{
    /// <summary>
    /// Represents the <see cref="IClientEndPoint"/> interface.
    /// </summary>
    public interface IClientEndPoint
    {
        /// <summary>
        /// Starts the client end point asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="messageProcessor">The message processor.</param>
        /// <param name="messageSerializer">The message serializer.</param>
        /// <returns>Returns an awaitable <see cref="Task"/>.</returns>
        Task StartAsync(CancellationToken cancellationToken, IMessageProcessor messageProcessor, IMessageSerializer messageSerializer);

        /// <summary>
        /// Stops the server asynchronously.
        /// </summary>
        /// <returns>Returns an awaitable <see cref="Task"/>.</returns>
        Task StopAsync();
    }
}
