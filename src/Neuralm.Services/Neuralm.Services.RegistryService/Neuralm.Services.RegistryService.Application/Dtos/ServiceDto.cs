using Neuralm.Services.RegistryService.Domain;
using System;

namespace Neuralm.Services.RegistryService.Application.Dtos
{
    /// <summary>
    /// Represents the <see cref="ServiceDto"/> of <see cref="Service"/> class.
    /// </summary>
    public class ServiceDto
    {
        /// <inheritdoc cref="Service.Id"/>
        public Guid Id { get; set; }

        /// <inheritdoc cref="Service.Name"/>
        public string Name { get; set; }

        /// <inheritdoc cref="Service.Host"/>
        public string Host { get; set; }
        
        /// <inheritdoc cref="Service.Port"/>
        public int Port { get; set; }

        /// <inheritdoc cref="Service.Start"/>
        public DateTime Start { get; set; }

        /// <inheritdoc cref="Service.End"/>
        public DateTime End { get; set; }
    }
}
