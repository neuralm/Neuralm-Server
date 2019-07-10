using Neuralm.Utilities.Concurrent;
using System;
using System.Threading;
using System.Threading.Tasks;
using Neuralm.Utilities.Observer;

namespace Neuralm.Application.Messages
{
    public sealed class MessageListener<T> : IObserver, IDisposable
    {
        private readonly AsyncConcurrentQueue<object> _messageQueue = new AsyncConcurrentQueue<object>();
        private IDisposable _unsubscriber;

        public void Subscribe(IObservable provider)
        {
            _unsubscriber = provider.Subscribe(typeof(T), this);
        }

        public void Unsubscribe()
        {
            Dispose();
        }

        public void OnError()
        {
            Console.WriteLine($"{GetType().FullName} OnError.");
        }

        public void OnNext(object value)
        {
            _messageQueue.Enqueue(value);
        }

        public async Task<T> ReceiveMessageAsync(CancellationToken cancellationToken)
        {
            return (T)await _messageQueue.DequeueAsync(cancellationToken);
        }

        public void Dispose()
        {
            _unsubscriber?.Dispose();
        }
    }
}
