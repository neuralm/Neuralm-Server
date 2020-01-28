using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Neuralm.Services.Common.Mapping
{
    /// <summary>
    /// Represents the <see cref="GenericServiceProvider"/> class an implementation of the <see cref="IGenericServiceProvider"/> interface.
    /// </summary>
    internal class GenericServiceProvider : IGenericServiceProvider
    {
        private readonly ServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes an instance of the <see cref="GenericServiceProvider"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        internal GenericServiceProvider(ServiceProvider serviceProvider)
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

        /// <inheritdoc cref="IAsyncDisposable.DisposeAsync()"/>
        public ValueTask DisposeAsync()
        {
            return _serviceProvider.DisposeAsync();
        }

        /// <inheritdoc cref="IDisposable.Dispose()"/>
        public void Dispose()
        {
            _serviceProvider.Dispose();
        }
    }
}
