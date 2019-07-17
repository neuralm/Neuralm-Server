using System;

namespace Neuralm.Utilities.Observer
{
    /// <summary>
    /// Represents the <see cref="ObserverUnsubscriber"/> class.
    /// </summary>
    public sealed class ObserverUnsubscriber : IDisposable
    {
        private readonly ObserverCollection _observers;
        private readonly IObserver _observer;

        /// <summary>
        /// Initializes an instance of the <see cref="ObserverUnsubscriber"/> class.
        /// </summary>
        /// <param name="observerCollection">The observer collection.</param>
        /// <param name="observer">The observer.</param>
        public ObserverUnsubscriber(ObserverCollection observerCollection, IObserver observer)
        {
            _observers = observerCollection;
            _observer = observer;
        }

        /// <summary>
        /// Disposes the <see cref="_observer"/> from the <see cref="_observers"/>.
        /// </summary>
        public void Dispose()
        {
            if (_observer != null)
                _observers.Remove(_observer);
        }
    }
}
