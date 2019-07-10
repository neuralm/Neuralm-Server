using System;

namespace Neuralm.Application.Messages.Responses
{
    public class DisableTrainingRoomResponse : Response
    {
        public DisableTrainingRoomResponse(Guid requestId, string message = "", bool success = false) : base(requestId, message, success)
        {

        }
    }
}
