using System;

namespace Neuralm.Domain.Entities.NEAT
{
    public class TrainingSession
    {
        public Guid Id { get; private set; }
        public DateTime StartedTimestamp { get; private set; }
        public DateTime EndedTimestamp { get; private set; }
        public Guid UserId { get; private set; }
        public Guid TrainingRoomId { get; private set; }
        public virtual TrainingRoom TrainingRoom { get; private set; }
        
        /// <summary>
        /// EFCore entity constructor IGNORE!
        /// </summary>
        protected TrainingSession()
        {
            
        }

        public TrainingSession(TrainingRoom trainingRoom, Guid userId)
        {
            Id = Guid.NewGuid();
            TrainingRoomId = trainingRoom.Id;
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
