using Neuralm.Services.Common.Messages.Abstractions;
using System;
using Neuralm.Services.Common.Messages;

namespace Neuralm.Services.TrainingRoomService.Messages
{
    /// <summary>
    /// Represents the <see cref="EndTrainingSessionRequest"/> class.
    /// </summary>
    [Message("Put", "/endTrainingSession", typeof(EndTrainingSessionResponse))]
    public class EndTrainingSessionRequest : Request
    {
        /// <summary>
        /// Gets the training session id.
        /// </summary>
        public Guid TrainingSessionId { get; set; }
    }
}
