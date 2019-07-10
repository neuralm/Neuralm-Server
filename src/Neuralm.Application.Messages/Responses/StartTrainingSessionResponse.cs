using System;
using Neuralm.Application.Messages.Dtos;

namespace Neuralm.Application.Messages.Responses
{
    public class StartTrainingSessionResponse : Response
    {
        public TrainingSessionDto TrainingSession { get; }

        public StartTrainingSessionResponse(Guid requestId, TrainingSessionDto trainingSession, string message, bool success = false) : base(requestId, message, success)
        {
            TrainingSession = trainingSession;
        }
    }
}
