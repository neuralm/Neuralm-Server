using Neuralm.Services.Common.Messages.Abstractions;
using System;

namespace Neuralm.Services.UserService.Messages
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

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterResponse"/> class.
        /// SERIALIZATION CONSTRUCTOR!
        /// </summary>
        public RegisterResponse()
        {
            
        }
    }
}
