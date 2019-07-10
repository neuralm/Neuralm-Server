using System;

namespace Neuralm.Utilities.Observer
{
    public interface IObservable
    {
        IDisposable Subscribe(Type type, IObserver observer);
    }
}
