using System;

namespace Neuralm.Application.Messages.Responses
{
    public class CreateTrainingRoomResponse : Response
    {
        public CreateTrainingRoomResponse(Guid requestId, bool success) : base(requestId, success)
        {

        }
    }
}
