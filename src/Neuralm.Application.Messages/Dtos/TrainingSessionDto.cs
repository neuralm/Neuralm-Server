using System;

namespace Neuralm.Application.Messages.Dtos
{
    public class TrainingSessionDto
    {
        public Guid Id { get; set; }
        public DateTime StartedTimestamp { get; set; }
        public DateTime EndedTimestamp { get; set; }
        public Guid UserId { get; set; }
        public TrainingRoomDto TrainingRoom { get; set; }
    }
}
