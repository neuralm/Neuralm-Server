namespace Neuralm.Domain.Entities.NEAT
{
    public class TrainingRoom
    {
        public int OwnerId { get; }
        public User Owner { get; }
        public TrainingRoomSettings TrainingRoomSettings { get; }

        public TrainingRoom(User owner, TrainingRoomSettings trainingRoomSettings)
        {
            Owner = owner;
            OwnerId = Owner.Id;
            TrainingRoomSettings = trainingRoomSettings;
        }
    }
}
