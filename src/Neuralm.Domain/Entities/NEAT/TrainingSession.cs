using System;

namespace Neuralm.Domain.Entities.NEAT
{
    public class TrainingSession
    {
        public Guid Id { get; }
        public DateTime StartedTimestamp { get; }
        public DateTime EndedTimestamp { get; private set; }
        public Guid UserId { get; }
        public TrainingRoom TrainingRoom { get; }

        public TrainingSession(TrainingRoom trainingRoom, Guid userId)
        {
            Id = Guid.NewGuid();
            StartedTimestamp = DateTime.UtcNow;
            UserId = userId;
            TrainingRoom = trainingRoom;
        }

        public void EndTrainingSession()
        {
            EndedTimestamp = DateTime.UtcNow;
        }
    }
}
