using System;
using System.Collections.Generic;
using Neuralm.Application.Messages.Dtos;

namespace Neuralm.Application.Messages.Responses
{
    public class GetEnabledTrainingRoomsResponse : Response
    {
        public IList<TrainingRoomDto> TrainingRooms { get; }

        public GetEnabledTrainingRoomsResponse(Guid requestId, IList<TrainingRoomDto> trainingRooms, string message = "", bool success = false) : base(requestId, message, success)
        {
            TrainingRooms = trainingRooms;
        }
    }
}
