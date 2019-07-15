using System;

namespace Neuralm.Application.Messages.Responses
{
    public class RegisterResponse : Response
    {
        public RegisterResponse(Guid requestId, string message = "", bool success = false) : base(requestId, message, success)
        {

        }
    }
}
