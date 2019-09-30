using System;

namespace Neuralm.Services.TrainingRoomService.Messages.Dtos
{
    /// <summary>
    /// Represents the data transfer object for the TrainingSession class.
    /// </summary>
    public class TrainingSessionDto
    {
        /// <summary>
        /// Gets and sets the id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets and sets the started time stamp.
        /// </summary>
        public DateTime StartedTimestamp { get; set; }

        /// <summary>
        /// Gets and sets the ended time stamp.
        /// </summary>
        public DateTime EndedTimestamp { get; set; }

        /// <summary>
        /// Gets and sets the user id.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets and sets the training room.
        /// </summary>
        public TrainingRoomDto TrainingRoom { get; set; }
    }
}
