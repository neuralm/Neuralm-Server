using System;

namespace Neuralm.Application.Messages.Responses
{
    /// <summary>
    /// Represents the <see cref="GetBestOrganismsResponse"/> class.
    /// </summary>
    public class GetBestOrganismsResponse : Response
    {
        /// <summary>
        /// Initializes an instance of the <see cref="GetBestOrganismsResponse"/> class.
        /// </summary>
        /// <param name="requestId">The request id.</param>
        /// <param name="message">The message.</param>
        /// <param name="success">The success flag.</param>
        public GetBestOrganismsResponse(Guid requestId, string message = "", bool success = false) : base(requestId, message, success)
        {

        }
    }
}
