using System;

namespace Neuralm.Mapping
{
    internal class GenericServiceProvider : IGenericServiceProvider
    {
        private readonly IServiceProvider _serviceProvider;

        internal GenericServiceProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public object GetService(Type serviceType)
        {
            return _serviceProvider.GetService(serviceType);
        }

        public TService GetService<TService>()
        {
            return (TService)GetService(typeof(TService));
        }
    }
}
