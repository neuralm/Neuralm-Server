using System;

namespace Neuralm.Utilities.Observer
{
    /// <summary>
    /// Represents the <see cref="IObservable"/> interface.
    /// </summary>
    public interface IObservable
    {
        /// <summary>
        /// Subscribes the observer to the observable.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="observer">The observer.</param>
        /// <returns></returns>
        IDisposable Subscribe(Type type, IObserver observer);
    }
}
