using Neuralm.Services.Common.Messages.Abstractions;
using Neuralm.Services.TrainingRoomService.Messages.Dtos;
using System;

namespace Neuralm.Services.TrainingRoomService.Messages
{
    /// <summary>
    /// Represents the <see cref="StartTrainingSessionResponse"/> class.
    /// </summary>
    public class StartTrainingSessionResponse : Response
    {
        /// <summary>
        /// Gets the training session.
        /// </summary>
        public TrainingSessionDto TrainingSession { get; set; }

        /// <summary>
        /// Initializes an instance of the <see cref="StartTrainingSessionResponse"/> class.
        /// </summary>
        /// <param name="requestId">The request id.</param>
        /// <param name="trainingSession">The training session.</param>
        /// <param name="message">The message.</param>
        /// <param name="success">The success flag.</param>
        public StartTrainingSessionResponse(Guid requestId, TrainingSessionDto trainingSession, string message, bool success = false) : base(requestId, message, success)
        {
            TrainingSession = trainingSession;
        }
        
        /// <summary>
        /// Initializes an instance of the <see cref="StartTrainingSessionResponse"/> class.
        /// SERIALIZATION CONSTRUCTOR.
        /// </summary>
        public StartTrainingSessionResponse()
        {
            
        }
    }
}
