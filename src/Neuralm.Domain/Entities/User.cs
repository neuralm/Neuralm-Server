using System;
using System.Collections.Generic;
using Neuralm.Domain.Entities.Authentication;
using Neuralm.Domain.Entities.NEAT;

namespace Neuralm.Domain.Entities
{
    /// <summary>
    /// Represents the <see cref="User"/> class.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets and sets the id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets and sets the username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets and sets the timestamp created.
        /// </summary>
        public DateTime TimestampCreated { get; set; }

        /// <summary>
        /// Gets and sets the collection of credentials.
        /// </summary>
        public virtual ICollection<Credential> Credentials { get; set; }

        /// <summary>
        /// Gets and sets the collection of training rooms.
        /// </summary>
        public virtual ICollection<TrainingRoom> TrainingRooms { get; set; }
    }
}
