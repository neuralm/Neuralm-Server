using System;

namespace Neuralm.Application.Messages.Responses
{
    public class PostBrainScoresResponse : Response
    {
        public PostBrainScoresResponse(Guid requestId, string message = "", bool success = false) : base(requestId, message, success)
        {

        }
    }
}
