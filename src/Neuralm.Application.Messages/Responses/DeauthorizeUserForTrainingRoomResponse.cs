using System;

namespace Neuralm.Application.Messages.Responses
{
    public class DeauthorizeUserForTrainingRoomResponse : Response
    {
        public DeauthorizeUserForTrainingRoomResponse(Guid requestId, bool success) : base(requestId, success)
        {

        }
    }
}
