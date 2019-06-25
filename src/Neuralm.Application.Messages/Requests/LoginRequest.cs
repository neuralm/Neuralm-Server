using System;

namespace Neuralm.Application.Messages.Requests
{
    public class LoginRequest : IRequest
    {
        public Guid Id { get; }
    }
}
