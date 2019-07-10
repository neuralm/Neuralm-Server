using System;

namespace Neuralm.Domain.Entities.Authentication
{
    public class Credential
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int CredentialTypeId { get; set; }
        public string Identifier { get; set; }
        public string Secret { get; set; }
        public string Extra { get; set; }
        public virtual User User { get; set; }
        public virtual CredentialType CredentialType { get; set; }
    }
}
