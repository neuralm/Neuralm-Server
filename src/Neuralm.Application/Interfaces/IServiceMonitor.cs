using System.Threading.Tasks;
using Neuralm.Application.Messages.Events;

namespace Neuralm.Application.Interfaces
{
    /// <summary>
    /// Represents the <see cref="IServiceMonitor{TService}"/> interface.
    /// </summary>
    /// <typeparam name="TService">The service type to monitor.</typeparam>
    public interface IServiceMonitor<TService> where TService : IService
    {
        /// <summary>
        /// Starts monitoring the service asynchronously.
        /// </summary>
        /// <returns>Returns an awaitable <see cref="Task"/>.</returns>
        Task StartMonitoringAsync();

        /// <summary>
        /// Gets the status update from the service.
        /// </summary>
        /// <returns>Returns an awaitable <see cref="Task"/> with type parameter <see cref="MonitorEvent"/>.</returns>
        Task<MonitorEvent> GetStatusUpdateAsync();

        /// <summary>
        /// Ends monitoring the service asynchronously.
        /// </summary>
        /// <returns>Returns an awaitable <see cref="Task"/>.</returns>
        Task EndMonitoringAsync();
    }
}
