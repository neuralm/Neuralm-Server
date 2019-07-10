using System;

namespace Neuralm.Application.Messages.Responses
{
    public class EndTrainingSessionResponse : Response
    {
        public EndTrainingSessionResponse(Guid requestId, string message, bool success = false) : base(requestId, message, success)
        {

        }
    }
}
