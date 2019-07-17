﻿using System;

namespace Neuralm.Application.Messages.Responses
{
    /// <summary>
    /// Represents the <see cref="PostBrainScoresResponse"/> class.
    /// </summary>
    public class PostBrainScoresResponse : Response
    {
        /// <summary>
        /// Initializes an instance of the <see cref="PostBrainScoresResponse"/> class.
        /// </summary>
        /// <param name="requestId">The request id.</param>
        /// <param name="message">The message.</param>
        /// <param name="success">The success flag.</param>
        public PostBrainScoresResponse(Guid requestId, string message = "", bool success = false) : base(requestId, message, success)
        {

        }
    }
}
