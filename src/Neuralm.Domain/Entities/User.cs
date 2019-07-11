using System;
using System.Collections.Generic;
using Neuralm.Domain.Entities.Authentication;
using Neuralm.Domain.Entities.NEAT;

namespace Neuralm.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public DateTime TimestampCreated { get; set; }
        public virtual ICollection<Credential> Credentials { get; set; }
        public virtual ICollection<TrainingRoom> TrainingRooms { get; set; }
    }
}
