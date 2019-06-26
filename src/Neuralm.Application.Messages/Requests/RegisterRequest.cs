using System;

namespace Neuralm.Application.Messages.Requests
{
    public class RegisterRequest : IRequest
    {
        public Guid Id { get; set; }
        public string CredentialTypeCode { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
