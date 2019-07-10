using System.Collections.Generic;

namespace Neuralm.Utilities.Observer
{
    public sealed class ObserverCollection : List<IObserver>
    {
        public ObserverCollection(IObserver observer)
        {
            Add(observer);
        }

        public void OnNextAll(object obj)
        {
            ForEach(observer => observer.OnNext(obj));
        }

        public void OnErrorAll()
        {
            ForEach(observer => observer.OnError());
        }
    }
}
