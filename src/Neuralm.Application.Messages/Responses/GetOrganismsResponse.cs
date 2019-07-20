using System;
using System.Collections.Generic;
using Neuralm.Application.Messages.Dtos;

namespace Neuralm.Application.Messages.Responses
{
    /// <summary>
    /// Represents the <see cref="GetOrganismsResponse"/> class.
    /// </summary>
    public class GetOrganismsResponse : Response
    {
        /// <summary>
        /// Gets the collection of organisms.
        /// </summary>
        public IEnumerable<OrganismDto> Organisms { get; }

        /// <summary>
        /// Initializes an instance of the <see cref="GetOrganismsResponse"/> class.
        /// </summary>
        /// <param name="requestId">The request id.</param>
        /// <param name="organisms">The organisms.</param>
        /// <param name="message">The message.</param>
        /// <param name="success">The success flag.</param>
        public GetOrganismsResponse(Guid requestId, IEnumerable<OrganismDto> organisms, string message = "", bool success = false) : base(requestId, message, success)
        {
            Organisms = organisms;
        }
    }
}
