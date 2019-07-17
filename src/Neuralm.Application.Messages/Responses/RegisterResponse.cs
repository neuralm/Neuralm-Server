using System;

namespace Neuralm.Application.Messages.Responses
{
    /// <summary>
    /// Represents the <see cref="RegisterResponse"/> class.
    /// </summary>
    public class RegisterResponse : Response
    {
        /// <summary>
        /// Initializes an instance of the <see cref="RegisterResponse"/> class.
        /// </summary>
        /// <param name="requestId">The request id.</param>
        /// <param name="message">The message.</param>
        /// <param name="success">The success flag.</param>
        public RegisterResponse(Guid requestId, string message = "", bool success = false) : base(requestId, message, success)
        {

        }
    }
}
