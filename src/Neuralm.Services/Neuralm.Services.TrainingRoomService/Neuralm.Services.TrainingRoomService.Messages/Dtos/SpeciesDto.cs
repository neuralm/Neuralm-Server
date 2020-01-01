using System;
using System.Collections.Generic;

namespace Neuralm.Services.TrainingRoomService.Messages.Dtos
{
    /// <summary>
    /// Represents the data transfer object for the species class.
    /// </summary>
    public class SpeciesDto
    {
        /// <summary>
        /// Gets and sets the id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets the list of organisms.
        /// </summary>
        public List<OrganismDto> Organisms { get; set; }

        /// <summary>
        /// Gets and sets the species score.
        /// </summary>
        public double SpeciesScore { get; set; }
        
        /// <summary>
        /// Gets and sets the training room id.
        /// </summary>
        public Guid TrainingRoomId { get; set; }
    }
}