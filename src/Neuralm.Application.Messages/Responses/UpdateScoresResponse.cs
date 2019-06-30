using System;

namespace Neuralm.Application.Messages.Responses
{
    public class UpdateScoresResponse : Response
    {
        public UpdateScoresResponse(Guid requestId, bool success) : base(requestId, success)
        {

        }
    }
}
