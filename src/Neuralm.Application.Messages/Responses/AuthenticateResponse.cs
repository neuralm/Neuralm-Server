using System;
using Neuralm.Domain.Enumerations;

namespace Neuralm.Application.Messages.Responses
{
    public class AuthenticateResponse : Response
    {
        public AuthenticateError Error { get; }
        public string AccessToken { get; set; }

        public AuthenticateResponse(Guid requestId, string accessToken = "", AuthenticateError error = AuthenticateError.None, bool success = false) : base(requestId, success)
        {
            AccessToken = accessToken;
            Error = error;
        }
    }
}
