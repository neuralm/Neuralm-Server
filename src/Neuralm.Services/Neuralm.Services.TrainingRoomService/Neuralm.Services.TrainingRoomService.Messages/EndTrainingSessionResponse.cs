using Neuralm.Services.Common.Messages.Abstractions;
using System;

namespace Neuralm.Services.TrainingRoomService.Messages
{
    /// <summary>
    /// Represents the <see cref="EndTrainingSessionResponse"/> class.
    /// </summary>
    public class EndTrainingSessionResponse : Response
    {
        /// <summary>
        /// Initializes an instance of the <see cref="EndTrainingSessionResponse"/> class.
        /// </summary>
        /// <param name="requestId">The request id.</param>
        /// <param name="message">The message.</param>
        /// <param name="success">The success flag.</param>
        public EndTrainingSessionResponse(Guid requestId, string message, bool success = false) : base(requestId, message, success)
        {

        }
    }
}
