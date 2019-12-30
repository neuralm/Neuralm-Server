using Neuralm.Services.Common.Domain;
using System;

namespace Neuralm.Services.TrainingRoomService.Domain
{
    /// <summary>
    /// Represents the <see cref="User"/> class.
    /// </summary>
    public class User : IEntity
    {
        /// <summary>
        /// Gets and sets the id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets and sets the username.
        /// </summary>
        public string Username { get; set; }
    }
}
