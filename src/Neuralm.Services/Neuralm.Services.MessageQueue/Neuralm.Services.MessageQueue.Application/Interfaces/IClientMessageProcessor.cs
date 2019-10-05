using System.Threading;
using System.Threading.Tasks;

namespace Neuralm.Services.MessageQueue.Application.Interfaces
{
    /// <summary>
    /// Represents the <see cref="IClientMessageProcessor"/> interface.
    /// </summary>
    public interface IClientMessageProcessor : IMessageProcessor
    {
        /// <summary>
        /// Starts the message client processor asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable <see cref="Task"/>.</returns>
        Task StartAsync(CancellationToken cancellationToken);
    }
}
