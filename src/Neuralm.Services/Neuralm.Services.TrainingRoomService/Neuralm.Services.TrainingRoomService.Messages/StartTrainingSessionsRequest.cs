using Neuralm.Services.Common.Messages.Abstractions;
using System;

namespace Neuralm.Services.TrainingRoomService.Messages
{
    /// <summary>
    /// Represents the <see cref="StartTrainingSessionRequest"/> class.
    /// </summary>
    public class StartTrainingSessionRequest : Request
    {
        /// <summary>
        /// Gets the user id.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets the training room id.
        /// </summary>
        public Guid TrainingRoomId { get; set; }

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
        
        /// <summary>
        /// Initializes an instance of the <see cref="StartTrainingSessionRequest"/> class.
        /// SERIALIZATION CONSTRUCTOR.
        /// </summary>
        public StartTrainingSessionRequest()
        {
            
        }
    }
}
