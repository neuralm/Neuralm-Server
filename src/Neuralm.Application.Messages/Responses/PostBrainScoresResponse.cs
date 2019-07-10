using System;

namespace Neuralm.Application.Messages.Responses
{
    public class PostBrainScoresResponse : Response
    {
        public PostBrainScoresResponse(Guid requestId, bool success) : base(requestId, success)
        {

        }
    }
}
