using Neuralm.Services.Common.Messages.Abstractions;
using Neuralm.Services.TrainingRoomService.Messages.Dtos;
using System;

namespace Neuralm.Services.TrainingRoomService.Messages
{
    /// <summary>
    /// Represents the <see cref="CreateTrainingRoomRequest"/> class.
    /// </summary>
    public class CreateTrainingRoomRequest : Request
    {
        /// <summary>
        /// Gets the owner id.
        /// </summary>
        public Guid OwnerId { get; set; }

        /// <summary>
        /// Gets the training room name.
        /// </summary>
        public string TrainingRoomName { get; set; }

        /// <summary>
        /// Gets the training room settings.
        /// </summary>
        public TrainingRoomSettingsDto TrainingRoomSettings { get; set; }
    }
}
