using System;

namespace Neuralm.Application.Messages.Responses
{
    public class GetBestOrganismsResponse : Response
    {
        public GetBestOrganismsResponse(Guid requestId, bool success) : base(requestId, success)
        {

        }
    }
}
