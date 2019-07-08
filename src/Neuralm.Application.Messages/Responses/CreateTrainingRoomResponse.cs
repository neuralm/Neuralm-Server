using System;

namespace Neuralm.Application.Messages.Responses
{
    public class CreateTrainingRoomResponse : Response
    {
        public Guid TrainingRoomId { get; }
        public string Message { get; }

        public CreateTrainingRoomResponse(Guid requestId, Guid trainingRoomId, string message, bool success = false) : base(requestId, success)
        {
            TrainingRoomId = trainingRoomId;
            Message = message;
        }
    }
}
