using Neuralm.Services.Common.Messages.Abstractions;
using System;
using Neuralm.Services.Common.Messages;

namespace Neuralm.Services.TrainingRoomService.Messages
{
    /// <summary>
    /// Represents the <see cref="StartTrainingSessionRequest"/> class.
    /// </summary>
    [Message("Put", "/startTrainingSession", typeof(StartTrainingSessionResponse))]
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
    }
}
