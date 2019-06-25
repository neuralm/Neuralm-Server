using System;

namespace Neuralm.Application.Messages.Requests
{
    public class RegisterRequest : IRequest
    {
        public Guid Id { get; }
    }
}
