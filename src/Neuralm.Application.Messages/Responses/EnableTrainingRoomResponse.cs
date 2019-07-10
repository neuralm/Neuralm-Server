using System;

namespace Neuralm.Application.Messages.Responses
{
    public class EnableTrainingRoomResponse : Response
    {
        public EnableTrainingRoomResponse(Guid requestId, string message = "", bool success = false) : base(requestId, message, success)
        {

        }
    }
}
