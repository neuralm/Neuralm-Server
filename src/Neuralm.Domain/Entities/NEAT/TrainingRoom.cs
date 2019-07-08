using System;

namespace Neuralm.Domain.Entities.NEAT
{
    public class TrainingRoom
    {
        public Guid Id { get; }
        public int OwnerId { get; }
        public User Owner { get; }
        public TrainingRoomSettings TrainingRoomSettings { get; }
        public string Name { get; }

        public TrainingRoom(User owner, string name, TrainingRoomSettings trainingRoomSettings)
        {
            Id = Guid.NewGuid();
            Owner = owner;
            OwnerId = Owner.Id;
            Name = name;
            TrainingRoomSettings = trainingRoomSettings;
        }
    }
}
