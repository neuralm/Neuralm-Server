using Neuralm.Services.UserService.Domain;
using System;
using System.ComponentModel.DataAnnotations;

namespace Neuralm.Services.UserService.Application.Dtos
{
    /// <summary>
    /// Represents the <see cref="UserDto"/> of the <see cref="User"/> class.
    /// </summary>
    public class UserDto
    {
        /// <inheritdoc cref="User.Id"/>
        public Guid Id { get; set; }

        /// <inheritdoc cref="User.Username"/>
        [Required, StringLength(45, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 3)]
        public string Username { get; set; }

        /// <inheritdoc cref="User.TimestampCreated"/>
        public DateTime TimestampCreated { get; set; }
    }
}
