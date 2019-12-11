using Neuralm.Services.Common.Messages.Abstractions;
using System;

namespace Neuralm.Services.UserService.Messages
{
    /// <summary>
    /// Represents the <see cref="AuthenticateResponse"/> class.
    /// </summary>
    public class AuthenticateResponse : Response
    {
        /// <summary>
        /// Gets the access token.
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Gets the user id.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Initializes an instance of the <see cref="AuthenticateResponse"/> class.
        /// </summary>
        /// <param name="requestId">The request id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="accessToken">The access token.</param>
        /// <param name="message">The message.</param>
        /// <param name="success">The success flag.</param>
        public AuthenticateResponse(Guid requestId, Guid userId, string accessToken = "", string message = "", bool success = false) : base(requestId, message, success)
        {
            AccessToken = accessToken;
            UserId = userId;
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticateResponse"/> class.
        /// SERIALIZATION CONSTRUCTOR!
        /// </summary>
        public AuthenticateResponse()
        {
            
        }
    }
}
