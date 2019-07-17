using System;

namespace Neuralm.Mapping
{
    /// <summary>
    /// Represents the <see cref="GenericServiceProvider"/> class an implementation of the <see cref="IGenericServiceProvider"/> interface.
    /// </summary>
    internal class GenericServiceProvider : IGenericServiceProvider
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes an instance of the <see cref="GenericServiceProvider"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        internal GenericServiceProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Gets the service by the provided service type.
        /// </summary>
        /// <param name="serviceType">The service type.</param>
        /// <returns>Returns a service by service type as <see cref="object"/>.</returns>
        public object GetService(Type serviceType)
        {
            return _serviceProvider.GetService(serviceType);
        }

        /// <inheritdoc cref="IGenericServiceProvider.GetService{TService}"/>
        public TService GetService<TService>()
        {
            return (TService)GetService(typeof(TService));
        }
    }
}
