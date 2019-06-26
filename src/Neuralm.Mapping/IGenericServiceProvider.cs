using System;

namespace Neuralm.Mapping
{
    public interface IGenericServiceProvider : IServiceProvider
    {
        TService GetService<TService>();
    }
}
