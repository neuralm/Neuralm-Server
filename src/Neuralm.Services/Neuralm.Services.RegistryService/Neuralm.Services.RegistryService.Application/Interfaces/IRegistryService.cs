using Neuralm.Services.Common.Application.Interfaces;
using Neuralm.Services.RegistryService.Application.Dtos;
using System.Threading;
using System.Threading.Tasks;

namespace Neuralm.Services.RegistryService.Application.Interfaces
{
    /// <summary>
    /// Represents the <see cref="IRegistryService"/> interface.
    /// </summary>
    public interface IRegistryService : IService<ServiceDto>
    {
        /// <summary>
        /// Starts up the registry service and checks which services are needed and launches them asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable <see cref="Task"/>.</returns>
        Task StartupAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Gets the service by name asynchronously.
        /// </summary>
        /// <param name="serviceName">The service name.</param>
        /// <returns>Returns the service dto by name.</returns>
        Task<ServiceDto> GetServiceByNameAsync(string serviceName);

        /// <summary>
        /// Starts to monitor the services asynchronously. 
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable <see cref="Task"/>.</returns>
        Task StartMonitoringAsync(CancellationToken cancellationToken);
    }
}
