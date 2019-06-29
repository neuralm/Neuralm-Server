using System;
using Neuralm.Domain.Enumerations;

namespace Neuralm.Application.Messages.Responses
{
    public class RegisterResponse : Response
    {
        public RegisterError Error { get; }

        public RegisterResponse(Guid requestId, RegisterError error = RegisterError.None, bool success = false) : base(requestId, success)
        {
            Error = error;
        }
    }
}
