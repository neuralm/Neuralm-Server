using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Neuralm.Utilities.Concurrent
{
    /// <summary>
    /// Represents the <see cref="AsyncConcurrentQueue{T}"/> class.
    /// </summary>
    /// <typeparam name="T">The generic type.</typeparam>
    public sealed class AsyncConcurrentQueue<T>
    {
        private readonly BlockingCollection<T> _blockingCollection = new BlockingCollection<T>();

        /// <summary>
        /// Enqueue an item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Enqueue(T item)
        {
            _blockingCollection.Add(item);
        }

        /// <summary>
        /// Dequeues an item asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable <see cref="Task"/> with type parameter of <see cref="T"/>.</returns>
        public Task<T> DequeueAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                while (true)
                    if (_blockingCollection.TryTake(out T item, -1, cancellationToken))
                        return item;
            }, cancellationToken);
        }
    }
}
