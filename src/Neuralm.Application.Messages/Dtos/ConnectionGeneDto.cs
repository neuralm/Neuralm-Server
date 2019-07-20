using System;
using Neuralm.Domain.Entities.NEAT;

namespace Neuralm.Application.Messages.Dtos
{
    /// <summary>
    /// Represents the data transfer object for the <see cref="ConnectionGene"/> class.
    /// </summary>
    public class ConnectionGeneDto
    {
        /// <inheritdoc cref="ConnectionGene.Id"/>
        public Guid Id { get; set; }

        /// <inheritdoc cref="ConnectionGene.BrainId"/>
        public Guid BrainId { get; set; }

        /// <inheritdoc cref="ConnectionGene.InId"/>
        public uint InId { get; set; }

        /// <inheritdoc cref="ConnectionGene.OutId"/>
        public uint OutId { get; set; }

        /// <inheritdoc cref="ConnectionGene.InnovationNumber"/>
        public uint InnovationNumber { get; set; }

        /// <inheritdoc cref="ConnectionGene.Weight"/>
        public double Weight { get; set; }

        /// <inheritdoc cref="ConnectionGene.Enabled"/>
        public bool Enabled { get; set; }
    }
}
