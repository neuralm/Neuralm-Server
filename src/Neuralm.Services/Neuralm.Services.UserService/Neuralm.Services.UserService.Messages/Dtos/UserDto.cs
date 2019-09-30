using System;
using System.ComponentModel.DataAnnotations;

namespace Neuralm.Services.UserService.Messages.Dtos
{
    /// <summary>
    /// Represents the <see cref="UserDto"/> of the User class.
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
        [Required, StringLength(45, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 3)]
        public string Username { get; set; }

        /// <summary>
        /// Gets and sets the time stamp created.
        /// </summary>
        public DateTime TimestampCreated { get; set; }
    }
}
