using System;

namespace Neuralm.Application.Messages.Responses
{
    public class EndTrainingSessionResponse : Response
    {
        public EndTrainingSessionResponse(Guid requestId, bool success) : base(requestId, success)
        {

        }
    }
}
