using System.Threading.Tasks;
using Neuralm.Application.Messages.Events;

namespace Neuralm.Application.Interfaces
{
    public interface IServiceMonitor<TService> where TService : IService
    {
        Task StartMonitoringAsync();
        Task<MonitorEvent> UpdateAsync();
        Task EndMonitoringAsync();
    }
}
