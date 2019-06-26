using System;
using Neuralm.Domain.Enumerations;

namespace Neuralm.Application.Messages.Responses
{
    public class AuthenticateResponse : IResponse
    {
        public Guid Id { get; }
        public Guid RequestId { get; }
        public AuthenticateError Error { get; }
        public string AccessToken { get; set; }
        public bool Success { get; }

        public AuthenticateResponse(Guid id, Guid requestId, string accessToken = "", AuthenticateError error = AuthenticateError.None, bool success = false)
        {
            Id = id;
            RequestId = requestId;
            AccessToken = accessToken;
            Error = error;
            Success = success;
        }
    }
}
