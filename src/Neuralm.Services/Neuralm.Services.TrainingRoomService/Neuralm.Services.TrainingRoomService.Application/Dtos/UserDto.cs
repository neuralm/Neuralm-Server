using Neuralm.Services.TrainingRoomService.Domain;
using System;

namespace Neuralm.Services.TrainingRoomService.Application.Dtos
{
    /// <summary>
    /// Represents the data transfer object for the <see cref="User"/> class.
    /// </summary>
    public class UserDto
    {
        /// <inheritdoc cref="User.Id"/>
        public Guid Id { get; set; }

        /// <inheritdoc cref="User.Username"/>
        public string Username { get; set; }

        /// <inheritdoc cref="User.TimestampCreated"/>
        public DateTime TimestampCreated { get; set; }
    }
}
