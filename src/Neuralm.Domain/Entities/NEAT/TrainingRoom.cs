namespace Neuralm.Domain.Entities.NEAT
{
    public class TrainingRoom
    {
        public TrainingRoomSettings TrainingRoomSettings { get; }

        public TrainingRoom(TrainingRoomSettings trainingRoomSettings)
        {
            TrainingRoomSettings = trainingRoomSettings;
        }
    }
}
