using System.Threading.Tasks;
using Neuralm.Application.Messages.Events;

namespace Neuralm.Application.Interfaces
{
    /// <summary>
    /// The interface for monitoring a service.
    /// </summary>
    /// <typeparam name="TService">The service type to monitor</typeparam>
    public interface IServiceMonitor<TService> where TService : IService
    {
        Task StartMonitoringAsync();
        Task<MonitorEvent> UpdateAsync();
        Task EndMonitoringAsync();
    }
}
