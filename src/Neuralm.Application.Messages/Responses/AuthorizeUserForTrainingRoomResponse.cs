using System;

namespace Neuralm.Application.Messages.Responses
{
    public class AuthorizeUserForTrainingRoomResponse : Response
    {
        public AuthorizeUserForTrainingRoomResponse(Guid requestId, string message = "", bool success = false) : base(requestId, message, success)
        {

        }
    }
}
