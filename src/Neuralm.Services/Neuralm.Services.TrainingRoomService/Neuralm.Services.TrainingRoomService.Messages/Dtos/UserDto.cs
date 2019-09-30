using System;

namespace Neuralm.Services.TrainingRoomService.Messages.Dtos
{
    /// <summary>
    /// Represents the data transfer object for the User class.
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// Gets and sets the id.
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Gets and sets the user name.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets and sets the time stamp created.
        /// </summary>
        public DateTime TimestampCreated { get; set; }
    }
}
