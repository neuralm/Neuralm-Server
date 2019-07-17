using System;
using Neuralm.Domain.Entities.NEAT;

namespace Neuralm.Application.Messages.Requests
{
    /// <summary>
    /// Represents the <see cref="CreateTrainingRoomRequest"/> class.
    /// </summary>
    public class CreateTrainingRoomRequest : Request
    {
        /// <summary>
        /// Gets the owner id.
        /// </summary>
        public Guid OwnerId { get; }

        /// <summary>
        /// Gets the training room name.
        /// </summary>
        public string TrainingRoomName { get; }

        /// <summary>
        /// Gets the training room settings.
        /// </summary>
        public TrainingRoomSettings TrainingRoomSettings { get; }

        /// <summary>
        /// Initializes an instance of the <see cref="CreateTrainingRoomRequest"/> class.
        /// </summary>
        /// <param name="ownerId">The owner id.</param>
        /// <param name="trainingRoomName">The training room name.</param>
        /// <param name="trainingRoomSettings">The training room settings.</param>
        public CreateTrainingRoomRequest(Guid ownerId, string trainingRoomName, TrainingRoomSettings trainingRoomSettings)
        {
            OwnerId = ownerId;
            TrainingRoomName = trainingRoomName;
            TrainingRoomSettings = trainingRoomSettings;
        }
    }
}
