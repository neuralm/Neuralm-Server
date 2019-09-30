using Neuralm.Services.Common.Messages.Abstractions;
using System;
using System.Collections.Generic;

namespace Neuralm.Services.TrainingRoomService.Messages
{
    /// <summary>
    /// Represents the <see cref="PostOrganismsScoreRequest"/> class.
    /// </summary>
    public class PostOrganismsScoreRequest : Request
    {
        /// <summary>
        /// Gets and sets the training session id.
        /// </summary>
        public Guid TrainingSessionId { get; }

        /// <summary>
        /// Gets and sets the organism scores dictionary.
        /// </summary>
        public Dictionary<Guid, double> OrganismScores { get; }

        /// <summary>
        /// Initializes an instance of the <see cref="PostOrganismsScoreRequest"/> class.
        /// </summary>
        /// <param name="trainingSessionId">The training session id.</param>
        /// <param name="organismScores">The organism to score dictionary.</param>
        public PostOrganismsScoreRequest(Guid trainingSessionId, Dictionary<Guid, double> organismScores)
        {
            TrainingSessionId = trainingSessionId;
            OrganismScores = organismScores;
        }
    }
}