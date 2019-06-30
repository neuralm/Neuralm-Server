using System;

namespace Neuralm.Application.Messages.Responses
{
    public class GetGenerationStatusResponse : Response
    {
        public GetGenerationStatusResponse(Guid requestId, bool success) : base(requestId, success)
        {

        }
    }
}
