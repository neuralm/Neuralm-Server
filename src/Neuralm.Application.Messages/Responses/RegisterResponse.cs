using System;
using Neuralm.Domain.Enumerations;

namespace Neuralm.Application.Messages.Responses
{
    public class RegisterResponse : Response
    {
        public RegisterError Error { get; }

        public RegisterResponse(Guid requestId, RegisterError error = RegisterError.None, string message = "", bool success = false) : base(requestId, message, success)
        {
            Error = error;
        }
    }
}
