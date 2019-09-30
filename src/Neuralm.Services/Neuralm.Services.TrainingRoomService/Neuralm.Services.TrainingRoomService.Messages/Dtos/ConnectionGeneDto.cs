using System;

namespace Neuralm.Services.TrainingRoomService.Messages.Dtos
{
    /// <summary>
    /// Represents the data transfer object for the ConnectionGene class.
    /// </summary>
    public class ConnectionGeneDto
    {
        /// <summary>
        /// Gets and sets the id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets and sets the organism id.
        /// </summary>
        public Guid OrganismId { get; set; }

        /// <summary>
        /// Gets and sets the in node identifier.
        /// </summary>
        public uint InNodeIdentifier { get; set; }

        /// <summary>
        /// Gets and sets the out node identifier.
        /// </summary>
        public uint OutNodeIdentifier { get; set; }

        /// <summary>
        /// Gets and sets the innovation number.
        /// </summary>
        public uint InnovationNumber { get; set; }
        
        /// <summary>
        /// Gets and sets the weight.
        /// </summary>
        public double Weight { get; set; }

        /// <summary>
        /// Gets and sets a value whether the connection gene is enabled.
        /// </summary>
        public bool Enabled { get; set; }
    }
}
