using System;

namespace Neuralm.Application.Messages.Responses
{
    /// <summary>
    /// Represents the <see cref="DisableTrainingRoomResponse"/> class.
    /// </summary>
    public class DisableTrainingRoomResponse : Response
    {
        /// <summary>
        /// Initializes an instance of the <see cref="DisableTrainingRoomResponse"/> class.
        /// </summary>
        /// <param name="requestId">The request id.</param>
        /// <param name="message">The message.</param>
        /// <param name="success">The success flag.</param>
        public DisableTrainingRoomResponse(Guid requestId, string message = "", bool success = false) : base(requestId, message, success)
        {

        }
    }
}
