using System;

namespace Neuralm.Application.Messages.Responses
{
    public class EnableTrainingRoomResponse : Response
    {
        public EnableTrainingRoomResponse(Guid requestId, bool success) : base(requestId, success)
        {

        }
    }
}
