using Neuralm.Services.TrainingRoomService.Domain;
using System;

namespace Neuralm.Services.TrainingRoomService.Application.Dtos
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
    }
}
