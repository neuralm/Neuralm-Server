using Neuralm.Services.Common.Messages.Abstractions;
using System;

namespace Neuralm.Services.RegistryService.Messages
{
    /// <summary>
    /// Represents the <see cref="RemoveServiceCommand"/> class.
    /// </summary>
    public class RemoveServiceCommand : Command
    {
        /// <summary>
        /// Gets and sets the service id.
        /// </summary>
        public Guid ServiceId { get; set; }
    }
}
