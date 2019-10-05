using System.Threading;
using System.Threading.Tasks;

namespace Neuralm.Services.MessageQueue.Application.Interfaces
{
    /// <summary>
    /// Represent the <see cref="IServiceConnector"/> interface.
    /// </summary>
    public interface IServiceConnector
    {
        /// <summary>
        /// Enqueue message in the service message queue.
        /// </summary>
        /// <param name="message">The message.</param>
        void EnqueueMessage(object message);

        /// <summary>
        /// Starts publishing messages from the message queue asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable <see cref="Task"/>.</returns>
        Task StartPublishingAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Adds a service to the service network connector dictionary.
        /// </summary>
        /// <param name="networkConnector">The network connector.</param>
        void AddService(INetworkConnector networkConnector);
    }
}
