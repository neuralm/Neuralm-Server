using System;
using System.Collections.Generic;

namespace Neuralm.Domain.Entities
{
    public class Room
    {
        public Guid Id { get; set; }
        public User Owner { get; set; }
        public string Name { get; }
        public List<Session> Sessions { get; }

        public Room(User owner, string name)
        {
            Id = Guid.NewGuid();
            Owner = owner;
            Name = name;
            Sessions = new List<Session>();
        }
    }
}
