namespace Neuralm.Services.Common.Observer
{
    /// <summary>
    /// Represents the <see cref="IObserver"/> interface.
    /// </summary>
    public interface IObserver
    {
        /// <summary>
        /// Subscribes the <see cref="IObserver"/> to the <see cref="IObservable"/> provider.
        /// </summary>
        /// <param name="provider"></param>
        void Subscribe(IObservable provider);

        /// <summary>
        /// Unsubscribes.
        /// </summary>
        void Unsubscribe();

        /// <summary>
        /// On error method callback.
        /// </summary>
        void OnError();

        /// <summary>
        /// On next method callback.
        /// </summary>
        /// <param name="value">The value.</param>
        void OnNext(object value);
    }
}
