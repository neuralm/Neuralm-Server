using System;

namespace Neuralm.Application.Messages.Responses
{
    /// <summary>
    /// Represents the <see cref="AuthorizeUserForTrainingRoomResponse"/> class.
    /// </summary>
    public class AuthorizeUserForTrainingRoomResponse : Response
    {
        /// <summary>
        /// Initializes an instance of the <see cref="AuthorizeUserForTrainingRoomResponse"/> class.
        /// </summary>
        /// <param name="requestId">The request id.</param>
        /// <param name="message">The message.</param>
        /// <param name="success">The success flag.</param>
        public AuthorizeUserForTrainingRoomResponse(Guid requestId, string message = "", bool success = false) : base(requestId, message, success)
        {

        }
    }
}
