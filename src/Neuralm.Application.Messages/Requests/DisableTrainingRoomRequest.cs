using System;

namespace Neuralm.Application.Messages.Requests
{
    /// <summary>
    /// Represents the <see cref="DisableTrainingRoomRequest"/> class.
    /// </summary>
    public class DisableTrainingRoomRequest : Request
    {
        /// <summary>
        /// Gets the training room id.
        /// </summary>
        public Guid TrainingRoomId { get; }

        /// <summary>
        /// Gets the user id.
        /// </summary>
        public Guid UserId { get; }

        /// <summary>
        /// Initializes an instance of the <see cref="DisableTrainingRoomRequest"/> class.
        /// </summary>
        /// <param name="trainingRoomId">The training room id.</param>
        /// <param name="userId">The user id.</param>
        public DisableTrainingRoomRequest(Guid trainingRoomId, Guid userId)
        {
            TrainingRoomId = trainingRoomId;
            UserId = userId;
        }
    }
}
