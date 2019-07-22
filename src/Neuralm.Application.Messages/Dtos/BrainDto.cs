using System;
using System.Collections.Generic;
using Neuralm.Domain.Entities.NEAT;

namespace Neuralm.Application.Messages.Dtos
{
    /// <summary>
    /// Represents the data transfer object for the <see cref="Brain"/> class.
    /// </summary>
    public class BrainDto
    {
        /// <inheritdoc cref="Brain.Id"/>
        public Guid Id { get; set; }

        /// <inheritdoc cref="Brain.TrainingRoomId"/>
        public Guid TrainingRoomId { get; set; }

        /// <inheritdoc cref="Brain.OrganismId"/>
        public Guid OrganismId { get; set; }

        /// <inheritdoc cref="Brain.ConnectionGenes"/>
        public List<ConnectionGeneDto> ConnectionGenes { get; set; }
    }
}
