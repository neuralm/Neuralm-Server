using System;

namespace Neuralm.Utilities.Observer
{
    public sealed class ObserverUnsubscriber : IDisposable
    {
        private readonly ObserverCollection _observers;
        private readonly IObserver _observer;

        /// <summary>
        /// Initializes an instance of the <see cref="ObserverUnsubscriber"/> class.
        /// </summary>
        /// <param name="observerCollection">The observer collection.</param>
        /// <param name="observer">The observer.</param>
        public ObserverUnsubscriber(ObserverCollection observers, IObserver observer)
        {
            _observers = observers;
            _observer = observer;
        }

        public void Dispose()
        {
            if (_observer != null)
                _observers.Remove(_observer);
        }
    }
}
