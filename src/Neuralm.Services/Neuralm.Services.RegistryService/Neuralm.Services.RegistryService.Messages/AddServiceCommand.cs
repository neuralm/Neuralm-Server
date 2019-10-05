using Neuralm.Services.Common.Messages.Abstractions;
using Neuralm.Services.RegistryService.Messages.Dtos;

namespace Neuralm.Services.RegistryService.Messages
{
    /// <summary>
    /// Represents the <see cref="AddServiceCommand"/> class.
    /// </summary>
    public class AddServiceCommand : Command
    {
        /// <summary>
        /// Gets and sets the service.
        /// </summary>
        public ServiceDto Service { get; set; }
    }
}
