using System;

namespace Neuralm.Application.Messages.Requests
{
    public class StartTrainingSessionRequest : Request
    {
        public Guid UserId { get; }
        public Guid TrainingRoomId { get; }

        public StartTrainingSessionRequest(Guid userId, Guid trainingRoomId)
        {
            UserId = userId;
            TrainingRoomId = trainingRoomId;
        }
    }
}
