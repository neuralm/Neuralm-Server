using System;
using Neuralm.Domain.Enumerations;

namespace Neuralm.Application.Messages.Responses
{
    public class RegisterResponse : IResponse
    {
        public Guid Id { get; }
        public Guid RequestId { get; }
        public RegisterError Error { get; }
        public bool Success { get; }

        public RegisterResponse(Guid id, Guid requestId, RegisterError error = RegisterError.None, bool success = false)
        {
            Id = id;
            RequestId = requestId;
            Error = error;
            Success = success;
        }
    }
}
