using System;

namespace Neuralm.Application.Messages.Responses
{
    public class RegisterResponse : IResponse
    {
        public Guid Id { get; }
        public Guid RequestId { get; }
    }
}
