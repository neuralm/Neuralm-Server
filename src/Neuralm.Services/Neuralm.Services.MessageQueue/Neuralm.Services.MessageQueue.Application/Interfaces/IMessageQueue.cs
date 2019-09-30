using System.Threading;
using System.Threading.Tasks;

namespace Neuralm.Services.MessageQueue.Application.Interfaces
{
    /// <summary>
    /// Represents the <see cref="IMessageQueue"/> interface.
    /// </summary>
    public interface IMessageQueue
    {
        /// <summary>
        /// Starts listening to messages asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="messageProcessor">The message processor.</param>
        /// <param name="messageSerializer">The message serializer.</param>
        /// <returns>Returns an awaitable <see cref="Task"/>.</returns>
        Task StartAsync(CancellationToken cancellationToken, IMessageProcessor messageProcessor, IMessageSerializer messageSerializer);

        /// <summary>
        /// Enqueue a message asynchronously.
        /// </summary>
        /// <returns>Returns an awaitable <see cref="Task"/>.</returns>
        Task EnqueueAsync();

        /// <summary>
        /// Dequeue a message asynchronously.
        /// </summary>
        /// <returns>Returns an awaitable <see cref="Task"/>.</returns>
        Task DequeueAsync();

        /// <summary>
        /// Stops the message queue.
        /// </summary>
        void Stop();
    }
}
