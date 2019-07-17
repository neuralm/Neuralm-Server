﻿using System;

namespace Neuralm.Application.Messages.Requests
{
    /// <summary>
    /// Represents the <see cref="EndTrainingSessionRequest"/> class.
    /// </summary>
    public class EndTrainingSessionRequest : Request
    {
        /// <summary>
        /// Gets the training session id.
        /// </summary>
        public Guid TrainingSessionId { get; }

        /// <summary>
        /// Initializes an instance of the <see cref="EndTrainingSessionRequest"/> class.
        /// </summary>
        /// <param name="trainingSessionId"></param>
        public EndTrainingSessionRequest(Guid trainingSessionId)
        {
            TrainingSessionId = trainingSessionId;
        }
    }
}
