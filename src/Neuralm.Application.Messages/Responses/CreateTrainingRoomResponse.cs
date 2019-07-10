using System;

namespace Neuralm.Application.Messages.Responses
{
    public class CreateTrainingRoomResponse : Response
    {
        public Guid TrainingRoomId { get; }

        public CreateTrainingRoomResponse(Guid requestId, Guid trainingRoomId, string message, bool success = false) : base(requestId, message, success)
        {
            TrainingRoomId = trainingRoomId;
        }
    }
}
