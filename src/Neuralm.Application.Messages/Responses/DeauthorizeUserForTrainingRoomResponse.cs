using System;

namespace Neuralm.Application.Messages.Responses
{
    public class DeauthorizeUserForTrainingRoomResponse : Response
    {
        public DeauthorizeUserForTrainingRoomResponse(Guid requestId, string message = "", bool success = false) : base(requestId, message, success)
        {

        }
    }
}
