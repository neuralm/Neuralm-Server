using Neuralm.Domain.Entities.NEAT;

namespace Neuralm.Application.Messages.Requests
{
    public class CreateTrainingRoomRequest : Request
    {
        public int OwnerId { get; }
        public string TrainingRoomName { get; }
        public TrainingRoomSettings TrainingRoomSettings { get; }

        public CreateTrainingRoomRequest(int ownerId, string trainingRoomName, TrainingRoomSettings trainingRoomSettings)
        {
            OwnerId = ownerId;
            TrainingRoomName = trainingRoomName;
            TrainingRoomSettings = trainingRoomSettings;
        }
    }
}
