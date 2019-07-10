using System;

namespace Neuralm.Application.Messages.Responses
{
    public class GetBestOrganismsResponse : Response
    {
        public GetBestOrganismsResponse(Guid requestId, string message = "", bool success = false) : base(requestId, message, success)
        {

        }
    }
}
