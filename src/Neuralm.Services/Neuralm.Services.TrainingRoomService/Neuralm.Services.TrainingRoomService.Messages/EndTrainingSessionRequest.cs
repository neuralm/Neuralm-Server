using Neuralm.Services.Common.Messages.Abstractions;
using System;

namespace Neuralm.Services.TrainingRoomService.Messages
{
    /// <summary>
    /// Represents the <see cref="EndTrainingSessionRequest"/> class.
    /// </summary>
    public class EndTrainingSessionRequest : Request
    {
        /// <summary>
        /// Gets the training session id.
        /// </summary>
        public Guid TrainingSessionId { get; set; }
    }
}
