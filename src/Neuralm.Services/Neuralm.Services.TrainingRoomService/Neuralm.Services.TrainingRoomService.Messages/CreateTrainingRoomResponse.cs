using Neuralm.Services.Common.Messages.Abstractions;
using System;

namespace Neuralm.Services.TrainingRoomService.Messages
{
    /// <summary>
    /// Represents the <see cref="CreateTrainingRoomResponse"/> class.
    /// </summary>
    public class CreateTrainingRoomResponse : Response
    {
        /// <summary>
        /// Gets the training room id.
        /// </summary>
        public Guid TrainingRoomId { get; }

        /// <summary>
        /// Initializes an instance of the <see cref="CreateTrainingRoomResponse"/> class.
        /// </summary>
        /// <param name="requestId">The request id.</param>
        /// <param name="trainingRoomId">The training room id.</param>
        /// <param name="message">The message.</param>
        /// <param name="success">The success flag.</param>
        public CreateTrainingRoomResponse(Guid requestId, Guid trainingRoomId, string message, bool success = false) : base(requestId, message, success)
        {
            TrainingRoomId = trainingRoomId;
        }
    }
}
