using System.Threading.Tasks;
using Neuralm.Services.Common.Messages.Dtos;

namespace Neuralm.Services.Common.Application.Interfaces
{
    /// <summary>
    /// Represents the <see cref="IRegistryService"/> interface.
    /// </summary>
    public interface IRegistryService
    {
        /// <summary>
        /// Adds the service asynchronously.
        /// </summary>
        /// <param name="service">The service to be added.</param>
        /// <returns>Returns whether the server was added successfully.</returns>
        Task<bool> AddServiceAsync(ServiceDto service);

        /// <summary>
        /// Gets the service asynchronously.
        /// </summary>
        /// <param name="serviceName">The service name.</param>
        /// <returns>Returns a service dto containing the requested service or null when not found.</returns>
        Task<ServiceDto> GetServiceAsync(string serviceName);
    }
}