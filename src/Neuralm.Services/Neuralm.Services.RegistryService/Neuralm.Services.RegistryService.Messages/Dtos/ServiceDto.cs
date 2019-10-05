using System;

namespace Neuralm.Services.RegistryService.Messages.Dtos
{
    /// <summary>
    /// Represents the data transfer object for the Service class.
    /// </summary>
    public class ServiceDto
    {
        /// <summary>
        /// Gets and sets the id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets and sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets and sets the host.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Gets and sets the port.
        /// </summary>
        public int Port { get; set; }
    }
}
