using System;

namespace Neuralm.Application.Messages.Responses
{
    public class StartTrainingSessionResponse : Response
    {
        public StartTrainingSessionResponse(Guid requestId, bool success) : base(requestId, success)
        {

        }
    }
}
