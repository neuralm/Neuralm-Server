using Neuralm.Services.TrainingRoomService.Domain;
using System;

namespace Neuralm.Services.TrainingRoomService.Application.Dtos
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
