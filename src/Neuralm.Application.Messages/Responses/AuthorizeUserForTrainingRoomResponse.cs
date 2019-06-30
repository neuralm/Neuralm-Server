using System;

namespace Neuralm.Application.Messages.Responses
{
    public class AuthorizeUserForTrainingRoomResponse : Response
    {
        public AuthorizeUserForTrainingRoomResponse(Guid requestId, bool success) : base(requestId, success)
        {

        }
    }
}
