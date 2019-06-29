using System;

namespace Neuralm.Application.Messages.Responses
{
    public class DisableTrainingRoomResponse : Response
    {
        public DisableTrainingRoomResponse(Guid requestId, bool success) : base(requestId, success)
        {

        }
    }
}
