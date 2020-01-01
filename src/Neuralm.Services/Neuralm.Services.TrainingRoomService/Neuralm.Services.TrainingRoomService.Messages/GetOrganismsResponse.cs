using Neuralm.Services.Common.Messages.Abstractions;
using Neuralm.Services.TrainingRoomService.Messages.Dtos;
using System;
using System.Collections.Generic;

namespace Neuralm.Services.TrainingRoomService.Messages
{
    /// <summary>
    /// Represents the <see cref="GetOrganismsResponse"/> class.
    /// </summary>
    public class GetOrganismsResponse : Response
    {
        /// <summary>
        /// Gets the collection of organisms.
        /// </summary>
        public List<OrganismDto> Organisms { get; set; }

        /// <summary>
        /// Initializes an instance of the <see cref="GetOrganismsResponse"/> class.
        /// </summary>
        /// <param name="requestId">The request id.</param>
        /// <param name="organisms">The organisms.</param>
        /// <param name="message">The message.</param>
        /// <param name="success">The success flag.</param>
        public GetOrganismsResponse(Guid requestId, List<OrganismDto> organisms, string message = "", bool success = false) : base(requestId, message, success)
        {
            Organisms = organisms ?? new List<OrganismDto>();
        }
        
        /// <summary>
        /// Initializes an instance of the <see cref="GetOrganismsResponse"/> class.
        /// SERIALIZATION CONSTRUCTOR.
        /// </summary>
        public GetOrganismsResponse()
        {
            
        }
    }
}
