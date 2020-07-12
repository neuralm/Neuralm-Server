using Neuralm.Services.Common.Domain;
using Neuralm.Services.Common.Observer;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Neuralm.Services.Common.Application
{
    /// <summary>
    /// Represents the <see cref="MessageListener{T}"/> class.
    /// </summary>
    /// <typeparam name="TMessage">The type of message.</typeparam>
    public abstract class MessageListener<TMessage> : IObserver, IDisposable
    {
        protected readonly AsyncConcurrentQueue<object> MessageQueue = new AsyncConcurrentQueue<object>();
        protected IDisposable Unsubscriber;

        /// <summary>
        /// Subscribes the message listener to an <see cref="IObservable"/> provider.
        /// </summary>
        /// <param name="provider">The provider.</param>
        public virtual void Subscribe(IObservable provider)
        {
            Unsubscriber = provider.Subscribe(typeof(TMessage), this);
        }

        /// <summary>
        /// Unsubscribes the message listener; i.e. disposes the subscription.
        /// </summary>
        public virtual void Unsubscribe()
        {
            Dispose();
        }

        /// <summary>
        /// Method to call on error.
        /// </summary>
        public abstract void OnError();

        /// <summary>
        /// Adds the message to the queue.
        /// </summary>
        /// <param name="value">The value; in this case the message.</param>
        public virtual void OnNext(object value)
        {
            MessageQueue.Enqueue(value);
        }

        /// <summary>
        /// Receive a message asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable <see cref="Task"/> with type parameter <see cref="TMessage"/>.</returns>
        public virtual async Task<TMessage> ReceiveMessageAsync(CancellationToken cancellationToken)
        {
            return (TMessage)await MessageQueue.DequeueAsync(cancellationToken);
        }

        /// <summary>
        /// Disposed the subscription.
        /// </summary>
        public virtual void Dispose()
        {
            Unsubscriber?.Dispose();
        }
    }
}
