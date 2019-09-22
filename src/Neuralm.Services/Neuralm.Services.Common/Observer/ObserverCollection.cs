using System.Collections.Generic;

namespace Neuralm.Services.Common.Observer
{
    /// <summary>
    /// Represents the <see cref="ObserverCollection"/> class.
    /// </summary>
    public sealed class ObserverCollection : List<IObserver>
    {
        /// <summary>
        /// Initializes an instance of the <see cref="ObserverCollection"/> class.
        /// </summary>
        /// <param name="observer">The first observer.</param>
        public ObserverCollection(IObserver observer)
        {
            Add(observer);
        }

        /// <summary>
        /// Calls all the <see cref="IObserver.OnNext"/> callbacks in the list.
        /// </summary>
        /// <param name="obj">The object.</param>
        public void OnNextAll(object obj)
        {
            ForEach(observer => observer.OnNext(obj));
        }

        /// <summary>
        /// Calls all the <see cref="IObserver.OnError"/> callbacks in the list.
        /// </summary>
        public void OnErrorAll()
        {
            ForEach(observer => observer.OnError());
        }
    }
}
