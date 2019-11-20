using System.Threading.Tasks;

namespace Neuralm.Services.Common.Application.Interfaces
{
    /// <summary>
    /// Represents the <see cref="IStartupService"/> interface.
    /// </summary>
    public interface IStartupService
    {
        /// <summary>
        /// Registers the service with the RegistryService.
        /// </summary>
        /// <returns>Returns an awaitable <see cref="Task"/>.</returns>
        Task RegisterServiceAsync(string serviceName, string host, int port);
    }
}