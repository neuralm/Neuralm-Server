using System;

namespace Neuralm.Mapping
{
    /// <summary>
    /// Represents the <see cref="IGenericServiceProvider"/> interface.
    /// </summary>
    public interface IGenericServiceProvider : IServiceProvider
    {
        /// <summary>
        /// Gets the service by type parameter.
        /// </summary>
        /// <typeparam name="TService">The service type.</typeparam>
        /// <returns>Returns a service by service type.</returns>
        TService GetService<TService>();
    }
}
