using Neuralm.Services.TrainingRoomService.Domain;
using System;
using System.Collections.Generic;

namespace Neuralm.Services.TrainingRoomService.Application.Dtos
{
    /// <summary>
    /// Represents the data transfer object for the <see cref="Organism"/> class.
    /// </summary>
    public class OrganismDto
    {
        /// <inheritdoc cref="Organism.Id"/>
        public Guid Id { get; set; }

        /// <inheritdoc cref="Organism.ConnectionGenes"/>
        public List<ConnectionGeneDto> ConnectionGenes { get; set; }

        /// <summary>
        /// Gets and sets the input nodes.
        /// </summary>
        public List<NodeDto> InputNodes { get; set; }

        /// <summary>
        /// Gets and sets the output nodes.
        /// </summary>
        public List<NodeDto> OutputNodes { get; set; }

        /// <inheritdoc cref="Organism.Score"/>
        public double Score { get; set; }

        /// <inheritdoc cref="Organism.Name"/>
        public string Name { get; set; }

        /// <inheritdoc cref="Organism.Generation"/>
        public uint Generation { get; set; }
    }
}
