using System;
using Neuralm.Domain.Entities.NEAT;

namespace Neuralm.Application.Messages.Dtos
{
    /// <summary>
    /// Represents the data transfer object for the <see cref="TrainingSession"/> class.
    /// </summary>
    public class TrainingSessionDto
    {
        /// <inheritdoc cref="TrainingSession.Id"/>
        public Guid Id { get; set; }

        /// <inheritdoc cref="TrainingSession.StartedTimestamp"/>
        public DateTime StartedTimestamp { get; set; }

        /// <inheritdoc cref="TrainingSession.EndedTimestamp"/>
        public DateTime EndedTimestamp { get; set; }

        /// <inheritdoc cref="TrainingSession.UserId"/>
        public Guid UserId { get; set; }

        /// <inheritdoc cref="TrainingSession.TrainingRoom"/>
        public TrainingRoomDto TrainingRoom { get; set; }
    }
}
