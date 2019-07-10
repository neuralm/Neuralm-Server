using System;

namespace Neuralm.Application.Messages.Requests
{
    public class EndTrainingSessionRequest : Request
    {
        public Guid TrainingSessionId { get; }

        public EndTrainingSessionRequest(Guid trainingSessionId)
        {
            TrainingSessionId = trainingSessionId;
        }
    }
}
