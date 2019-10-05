using Neuralm.Services.Common.Messages.Abstractions;
using Neuralm.Services.RegistryService.Messages.Dtos;
using System.Collections.Generic;

namespace Neuralm.Services.RegistryService.Messages
{
    /// <summary>
    /// Represents the <see cref="AddServicesCommand"/> class.
    /// </summary>
    public class AddServicesCommand : Command
    {
        /// <summary>
        /// Gets and sets the list of services.
        /// </summary>
        public List<ServiceDto> Services { get; set; }
    }
}
