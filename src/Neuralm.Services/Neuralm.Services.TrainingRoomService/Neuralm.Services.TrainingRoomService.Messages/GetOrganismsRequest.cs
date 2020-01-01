using System;
using Neuralm.Services.Common.Messages;
using Neuralm.Services.Common.Messages.Abstractions;

namespace Neuralm.Services.TrainingRoomService.Messages
{
    /// <summary>
    /// Represents the <see cref="GetOrganismsRequest"/> class.
    /// </summary>
    [Message("Get", "organisms", typeof(GetOrganismsResponse))]
    public class GetOrganismsRequest: Request
    {
        /// <summary>
        /// Gets and sets the training session id.
        /// </summary>
        public Guid TrainingSessionId { get; set; }
        
        /// <summary>
        /// Gets and sets the amount of organisms to be requested.
        /// </summary>
        public int Amount { get; set; }
        
        /// <summary>
        /// Gets and sets the user id.
        /// </summary>
        public Guid UserId { get; set; }
    }
}