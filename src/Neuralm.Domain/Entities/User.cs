using System;
using System.Collections.Generic;
using Neuralm.Domain.Entities.Authentication;

namespace Neuralm.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public DateTime TimestampCreated { get; set; }
        public virtual ICollection<Credential> Credentials { get; set; }
    }
}
