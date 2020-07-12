using Neuralm.Services.RegistryService.Messages;
using System.Threading;
using System.Threading.Tasks;

namespace Neuralm.Services.MessageQueue.Application.Interfaces
{
    /// <summary>
    /// Represents the <see cref="IRegistryService"/> interface.
    /// </summary>
    public interface IRegistryService
    {
        /// <summary>
        /// Starts receiving service end points asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable <see cref="Task"/>.</returns>
        Task StartReceivingServiceEndPointsAsync(CancellationToken cancellationToken);


        /// <summary>
        /// Starts monitoring known services with health checks.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable <see cref="Task"/>.</returns>
        Task StartMonitoringServicesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Adds services.
        /// </summary>
        /// <param name="addServicesCommand">The add services command.</param>
        /// <returns>Returns an awaitable <see cref="Task"/>.</returns>
        Task AddServices(AddServicesCommand addServicesCommand);

        /// <summary>
        /// Adds a service.
        /// </summary>
        /// <param name="addServiceCommand">The add service command.</param>
        /// <returns>Returns an awaitable <see cref="Task"/>.</returns>
        Task AddService(AddServiceCommand addServiceCommand);

        /// <summary>
        /// Removes a service.
        /// </summary>
        /// <param name="removeServiceCommand">The remove service command.</param>
        /// <returns></returns>
        Task RemoveService(RemoveServiceCommand removeServiceCommand);
    }
}
