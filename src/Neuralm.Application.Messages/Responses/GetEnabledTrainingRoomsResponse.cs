using System;
using System.Collections.Generic;
using Neuralm.Application.Messages.Dtos;

namespace Neuralm.Application.Messages.Responses
{
    /// <summary>
    /// Represents the <see cref="GetEnabledTrainingRoomsResponse"/> class.
    /// </summary>
    public class GetEnabledTrainingRoomsResponse : Response
    {
        /// <summary>
        /// Gets the training rooms. 
        /// </summary>
        public IList<TrainingRoomDto> TrainingRooms { get; }

        /// <summary>
        /// Initializes an instance of the <see cref="GetEnabledTrainingRoomsResponse"/> class.
        /// </summary>
        /// <param name="requestId">The request id.</param>
        /// <param name="trainingRooms">The training rooms.</param>
        /// <param name="message">The message.</param>
        /// <param name="success">The success flag.</param>
        public GetEnabledTrainingRoomsResponse(Guid requestId, IList<TrainingRoomDto> trainingRooms, string message = "", bool success = false) : base(requestId, message, success)
        {
            TrainingRooms = trainingRooms;
        }
    }
}
