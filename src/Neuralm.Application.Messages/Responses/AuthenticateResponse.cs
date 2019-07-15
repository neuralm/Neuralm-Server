using System;

namespace Neuralm.Application.Messages.Responses
{
    public class AuthenticateResponse : Response
    {
        public string AccessToken { get; set; }
        public Guid UserId { get; set; }

        public AuthenticateResponse(Guid requestId, Guid userId, string accessToken = "", string message = "", bool success = false) : base(requestId, message, success)
        {
            AccessToken = accessToken;
            UserId = userId;
        }
    }
}
