using System;
using System.Collections.Generic;

namespace Neuralm.Services.TrainingRoomService.Messages.Dtos
{
    /// <summary>
    /// Represents the data transfer object for the TrainingRoom class.
    /// </summary>
    public class TrainingRoomDto
    {
        /// <summary>
        /// Gets and sets the id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets and sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets and sets the owner id.
        /// </summary>
        public Guid OwnerId { get; set; }
        
        /// <summary>
        /// Gets and sets the owner.
        /// </summary>
        public UserDto Owner { get; set; }

        /// <summary>
        /// Gets and sets the generation.
        /// </summary>
        public uint Generation { get; set; }

        /// <summary>
        /// Gets and sets the training room settings.
        /// </summary>
        public TrainingRoomSettingsDto TrainingRoomSettings { get; set; }
        
        /// <summary>
        /// Gets and sets the list of training sessions.
        /// </summary>
        public List<TrainingSessionDto> TrainingSessions { get; set; }
        
        /// <summary>
        /// Gets and sets the list of authorized trainers.
        /// </summary>
        public List<TrainerDto> AuthorizedTrainers { get; set; }
    }
}
