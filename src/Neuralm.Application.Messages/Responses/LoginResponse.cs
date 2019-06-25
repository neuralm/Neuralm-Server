using System;

namespace Neuralm.Application.Messages.Responses
{
    public class LoginResponse : IResponse
    {
        public Guid Id { get; }
        public Guid RequestId { get; }
    }
}
