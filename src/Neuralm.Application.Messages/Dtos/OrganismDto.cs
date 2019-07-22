using System;
using Neuralm.Domain.Entities.NEAT;

namespace Neuralm.Application.Messages.Dtos
{
    /// <summary>
    /// Represents the data transfer object for the <see cref="Organism"/> class.
    /// </summary>
    public class OrganismDto
    {
        /// <inheritdoc cref="Organism.Id"/>
        public Guid Id { get; set; }

        /// <inheritdoc cref="Organism.SpeciesId"/>
        public Guid SpeciesId { get; set; }

        /// <inheritdoc cref="Organism.BrainId"/>
        public Guid BrainId { get; set; }

        /// <inheritdoc cref="Organism.TrainingRoomId"/>
        public Guid TrainingRoomId { get; set; }

        /// <inheritdoc cref="Organism.Brain"/>
        public BrainDto Brain { get; set; }

        /// <inheritdoc cref="Organism.Score"/>
        public double Score { get; set; }

        /// <inheritdoc cref="Organism.Name"/>
        public string Name { get; set; }

        /// <inheritdoc cref="Organism.Generation"/>
        public uint Generation { get; set; }
    }
}
