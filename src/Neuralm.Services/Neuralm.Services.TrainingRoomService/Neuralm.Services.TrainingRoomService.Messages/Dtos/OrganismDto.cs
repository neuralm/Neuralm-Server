using System;
using System.Collections.Generic;

namespace Neuralm.Services.TrainingRoomService.Messages.Dtos
{
    /// <summary>
    /// Represents the data transfer object for the Organism class.
    /// </summary>
    public class OrganismDto
    {
        /// <summary>
        /// Gets and sets the id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets and sets the id.
        /// </summary>
        public Guid SpeciesId { get; set; }

        /// <summary>
        /// Gets and sets the list of connection genes.
        /// </summary>
        public List<ConnectionGeneDto> ConnectionGenes { get; set; }

        /// <summary>
        /// Gets and sets the input nodes.
        /// </summary>
        public List<NodeDto> InputNodes { get; set; }

        /// <summary>
        /// Gets and sets the output nodes.
        /// </summary>
        public List<NodeDto> OutputNodes { get; set; }

        /// <summary>
        /// Gets and sets the score.
        /// </summary>
        public double Score { get; set; }

        /// <summary>
        /// Gets and sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets and sets the generation.
        /// </summary>
        public uint Generation { get; set; }
    }
}
