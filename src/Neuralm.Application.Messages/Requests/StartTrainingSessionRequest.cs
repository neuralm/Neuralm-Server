using System;

namespace Neuralm.Application.Messages.Requests
{
    /// <summary>
    /// Represents the <see cref="StartTrainingSessionRequest"/> class.
    /// </summary>
    public class StartTrainingSessionRequest : Request
    {
        /// <summary>
        /// Gets the user id.
        /// </summary>
        public Guid UserId { get; }

        /// <summary>
        /// Gets the training room id.
        /// </summary>
        public Guid TrainingRoomId { get; }

        /// <summary>
        /// Initializes an instance of the <see cref="StartTrainingSessionRequest"/> class.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="trainingRoomId">The training room id.</param>
        public StartTrainingSessionRequest(Guid userId, Guid trainingRoomId)
        {
            UserId = userId;
            TrainingRoomId = trainingRoomId;
        }
    }
}
