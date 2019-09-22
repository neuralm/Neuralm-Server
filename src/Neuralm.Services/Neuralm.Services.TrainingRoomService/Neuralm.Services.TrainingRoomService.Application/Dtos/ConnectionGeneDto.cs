using Neuralm.Services.TrainingRoomService.Domain;
using System;

namespace Neuralm.Services.TrainingRoomService.Application.Dtos
{
    /// <summary>
    /// Represents the data transfer object for the <see cref="ConnectionGene"/> class.
    /// </summary>
    public class ConnectionGeneDto
    {
        /// <inheritdoc cref="ConnectionGene.Id"/>
        public Guid Id { get; set; }

        /// <inheritdoc cref="ConnectionGene.OrganismId"/>
        public Guid OrganismId { get; set; }

        /// <inheritdoc cref="ConnectionGene.InNodeIdentifier"/>
        public uint InNodeIdentifier { get; set; }

        /// <inheritdoc cref="ConnectionGene.OutNodeIdentifier"/>
        public uint OutNodeIdentifier { get; set; }

        /// <inheritdoc cref="ConnectionGene.InnovationNumber"/>
        public uint InnovationNumber { get; set; }

        /// <inheritdoc cref="ConnectionGene.Weight"/>
        public double Weight { get; set; }

        /// <inheritdoc cref="ConnectionGene.Enabled"/>
        public bool Enabled { get; set; }
    }
}
