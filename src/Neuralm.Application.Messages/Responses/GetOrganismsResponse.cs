using System;

namespace Neuralm.Application.Messages.Responses
{
    public class GetOrganismsResponse : Response
    {
        public GetOrganismsResponse(Guid requestId, bool success) : base(requestId, success)
        {

        }
    }
}
