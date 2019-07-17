using System;
using Neuralm.Domain.Entities.NEAT;

namespace Neuralm.Application.Messages.Dtos
{
    /// <summary>
    /// Represents the data transfer object for the <see cref="TrainingRoom"/> class.
    /// </summary>
    public class TrainingRoomDto
    {
        /// <inheritdoc cref="TrainingRoom.Id"/>
        public Guid Id { get; set; }

        /// <inheritdoc cref="TrainingRoom.Name"/>
        public string Name { get; set; }

        /// <inheritdoc cref="TrainingRoom.Owner"/>
        public UserDto Owner { get; set; }

        /// <inheritdoc cref="TrainingRoom.Generation"/>
        public uint Generation { get; set; }

        /// <inheritdoc cref="TrainingRoom.TrainingRoomSettings"/>
        public TrainingRoomSettingsDto TrainingRoomSettings { get; set; }

        /// <inheritdoc cref="TrainingRoom.HighestScore"/>
        public double HighestScore { get; set; }

        /// <inheritdoc cref="TrainingRoom.LowestScore"/>
        public double LowestScore { get; set; }

        /// <inheritdoc cref="TrainingRoom.AverageScore"/>
        public double AverageScore { get; set; }
    }
}
