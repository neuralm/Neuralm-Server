using System;

namespace Neuralm.Application.Messages.Responses
{
    public class GetGenerationStatusResponse : Response
    {
        public GetGenerationStatusResponse(Guid requestId, string message = "", bool success = false) : base(requestId, message, success)
        {

        }
    }
}
