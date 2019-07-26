using System;

namespace Neuralm.Domain.Entities.NEAT
{
    /// <summary>
    /// Represents the <see cref="Trainer"/> class.
    /// </summary>
    public class Trainer
    {
        /// <summary>
        /// Gets and sets the user id.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets and sets the user.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Gets and sets the training room id.
        /// </summary>
        public Guid TrainingRoomId { get; set; }

        /// <summary>
        /// Gets and sets the training room.
        /// </summary>
        public virtual TrainingRoom TrainingRoom { get; set; }

        /// <summary>
        /// EFCore entity constructor.
        /// </summary>
        protected Trainer()
        {

        }

        /// <summary>
        /// Initializes an instance of the <see cref="Trainer"/> class.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="trainingRoom">The training room/</param>
        public Trainer(User user, TrainingRoom trainingRoom)
        {
            User = user;
            UserId = user.Id;
            TrainingRoom = trainingRoom;
            TrainingRoomId = trainingRoom.Id;
        }
    }
}
