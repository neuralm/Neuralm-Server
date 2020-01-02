using Neuralm.Services.Common.Messages.Abstractions;
using System;
using System.Collections.Generic;
using Neuralm.Services.Common.Messages;

namespace Neuralm.Services.TrainingRoomService.Messages
{
    /// <summary>
    /// Represents the <see cref="PostOrganismsScoreRequest"/> class.
    /// </summary>
    [Message("Patch", "/organisms", typeof(PostOrganismsScoreResponse))]
    public class PostOrganismsScoreRequest : Request
    {
        /// <summary>
        /// Gets and sets the training session id.
        /// </summary>
        public Guid TrainingSessionId { get; set; }

        /// <summary>
        /// Gets and sets the organism scores dictionary.
        /// </summary>
        public Dictionary<Guid, double> OrganismScores { get; set; }
    }
}