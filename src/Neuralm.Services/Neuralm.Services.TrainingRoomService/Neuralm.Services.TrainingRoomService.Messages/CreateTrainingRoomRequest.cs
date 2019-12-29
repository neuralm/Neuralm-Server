using Neuralm.Services.Common.Messages.Abstractions;
using Neuralm.Services.TrainingRoomService.Messages.Dtos;
using Neuralm.Services.Common.Messages;

namespace Neuralm.Services.TrainingRoomService.Messages
{
    /// <summary>
    /// Represents the <see cref="CreateTrainingRoomRequest"/> class.
    /// </summary>
    [Message("Post", "/", typeof(CreateTrainingRoomResponse))]
    public class CreateTrainingRoomRequest : Request
    {
        /// <summary>
        /// Gets the training room name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets and sets the owner.
        /// </summary>
        public UserDto Owner { get; set; }

        /// <summary>
        /// Gets the training room settings.
        /// </summary>
        public TrainingRoomSettingsDto TrainingRoomSettings { get; set; }
    }
}
