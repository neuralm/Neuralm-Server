using System;
using Neuralm.Domain.Entities.NEAT;

namespace Neuralm.Application.Messages.Requests
{
    public class CreateTrainingRoomRequest : Request
    {
        public Guid OwnerId { get; }
        public string TrainingRoomName { get; }
        public TrainingRoomSettings TrainingRoomSettings { get; }

        public CreateTrainingRoomRequest(Guid ownerId, string trainingRoomName, TrainingRoomSettings trainingRoomSettings)
        {
            OwnerId = ownerId;
            TrainingRoomName = trainingRoomName;
            TrainingRoomSettings = trainingRoomSettings;
        }
    }
}
