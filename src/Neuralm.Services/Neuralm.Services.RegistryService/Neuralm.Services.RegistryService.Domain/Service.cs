using Neuralm.Services.Common.Domain;
using System;

namespace Neuralm.Services.RegistryService.Domain
{
    /// <summary>
    /// Represents the <see cref="Service"/> class.
    /// </summary>
    public class Service : IEntity
    {
        /// <inheritdoc cref="IEntity.Id"/>
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

        /// <summary>
        /// Gets and sets the start date time.
        /// </summary>
        public DateTime Start { get; set; }

        /// <summary>
        /// Gets and sets the end date time.
        /// </summary>
        public DateTime End { get; set; }

        /// <summary>
        /// Gets and sets a value whether the service is alive.
        /// </summary>
        public bool IsAlive { get; set; }
    }
}
