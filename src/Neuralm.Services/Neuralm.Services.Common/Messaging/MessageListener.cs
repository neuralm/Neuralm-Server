using Neuralm.Services.Common.Concurrent;
using Neuralm.Services.Common.Observer;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Neuralm.Services.Common.Messaging
{
    /// <summary>
    /// Represents the <see cref="MessageListener{T}"/> class.
    /// </summary>
    /// <typeparam name="TMessage">The type of message.</typeparam>
    public sealed class MessageListener<TMessage> : IObserver, IDisposable
    {
        private readonly AsyncConcurrentQueue<object> _messageQueue = new AsyncConcurrentQueue<object>();
        private IDisposable _unsubscriber;
        
        /// <summary>
        /// Subscribes the message listener to an <see cref="IObservable"/> provider.
        /// </summary>
        /// <param name="provider">The provider.</param>
        public void Subscribe(IObservable provider)
        {
            _unsubscriber = provider.Subscribe(typeof(TMessage), this);
        }

        /// <summary>
        /// Unsubscribes the message listener; i.e. disposes the subscription.
        /// </summary>
        public void Unsubscribe()
        {
            Dispose();
        }

        /// <summary>
        /// Method to call on error.
        /// </summary>
        public void OnError()
        {
            Console.WriteLine($"{GetType().FullName}: OnError called.");
        }

        /// <summary>
        /// Adds the message to the queue.
        /// </summary>
        /// <param name="value">The value; in this case the message.</param>
        public void OnNext(object value)
        {
            _messageQueue.Enqueue(value);
        }

        /// <summary>
        /// Receive a message asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable <see cref="Task"/> with type parameter <see cref="TMessage"/>.</returns>
        public async Task<TMessage> ReceiveMessageAsync(CancellationToken cancellationToken)
        {
            return (TMessage)await _messageQueue.DequeueAsync(cancellationToken);
        }

        /// <summary>
        /// Disposed the subscription.
        /// </summary>
        public void Dispose()
        {
            _unsubscriber?.Dispose();
        }
    }
}
