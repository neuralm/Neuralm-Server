using Neuralm.Services.Common.Messages.Abstractions;
using System;

namespace Neuralm.Services.TrainingRoomService.Messages
{
    /// <summary>
    /// Represents the <see cref="PostOrganismsScoreResponse"/> class.
    /// </summary>
    public class PostOrganismsScoreResponse : Response
    {
        /// <summary>
        /// Initializes an instance of the <see cref="PostOrganismsScoreResponse"/> class.
        /// </summary>
        /// <param name="requestId">The request id.</param>
        /// <param name="message">The message.</param>
        /// <param name="success">The success flag.</param>
        public PostOrganismsScoreResponse(Guid requestId, string message = "", bool success = false) : base(requestId, message, success)
        {

        }
    }
}
