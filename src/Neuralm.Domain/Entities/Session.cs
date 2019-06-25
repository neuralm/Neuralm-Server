using System;

namespace Neuralm.Domain.Entities
{
    public class Session
    {
        public Guid Id { get; set; }
        public User Owner { get; set; }
        public Room Room { get; set; }

        public Session(Room room, User user)
        {
            Id = Guid.NewGuid();
            Room = room;
            Owner = user;
        }
    }
}
