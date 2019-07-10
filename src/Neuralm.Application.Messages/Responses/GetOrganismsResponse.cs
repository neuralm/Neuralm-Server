using System;

namespace Neuralm.Application.Messages.Responses
{
    public class GetOrganismsResponse : Response
    {
        public GetOrganismsResponse(Guid requestId, string message = "", bool success = false) : base(requestId, message, success)
        {

        }
    }
}
