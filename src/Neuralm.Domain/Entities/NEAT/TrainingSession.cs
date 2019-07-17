using System;

namespace Neuralm.Domain.Entities.NEAT
{
    /// <summary>
    /// Represents the <see cref="TrainingSession"/> class used for starting and ending a training session in a <see cref="NEAT.TrainingRoom"/>.
    /// </summary>
    public class TrainingSession
    {
        /// <summary>
        /// Gets and sets the id.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets and sets the started time stamp.
        /// </summary>
        public DateTime StartedTimestamp { get; private set; }

        /// <summary>
        /// Gets and sets the ended time stamp.
        /// </summary>
        public DateTime EndedTimestamp { get; private set; }

        /// <summary>
        /// Gets and sets the user id.
        /// </summary>
        public Guid UserId { get; private set; }

        /// <summary>
        /// Gets and sets the training room id.
        /// </summary>
        public Guid TrainingRoomId { get; private set; }

        /// <summary>
        /// Gets and sets the training room.
        /// </summary>
        public virtual TrainingRoom TrainingRoom { get; private set; }
        
        /// <summary>
        /// EFCore entity constructor IGNORE!
        /// </summary>
        protected TrainingSession()
        {
            
        }

        /// <summary>
        /// Initializes an instance of the <see cref="TrainingSession"/> class.
        /// </summary>
        /// <param name="trainingRoom">The training room.</param>
        /// <param name="userId">The user id.</param>
        public TrainingSession(TrainingRoom trainingRoom, Guid userId)
        {
            Id = Guid.NewGuid();
            TrainingRoomId = trainingRoom.Id;
            StartedTimestamp = DateTime.UtcNow;
            UserId = userId;
            TrainingRoom = trainingRoom;
        }

        /// <summary>
        /// Ends the training session; i.e. sets the <see cref="EndedTimestamp"/> to current date and time utc.
        /// </summary>
        public void EndTrainingSession()
        {
            EndedTimestamp = DateTime.UtcNow;
        }
    }
}
