using Neuralm.Services.Common.Domain;
using Neuralm.Services.UserService.Domain.Authentication;
using System;
using System.Collections.Generic;

namespace Neuralm.Services.UserService.Domain
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

        /// <summary>
        /// Gets and sets the timestamp created.
        /// </summary>
        public DateTime TimestampCreated { get; set; }

        /// <summary>
        /// Gets and sets the collection of credentials.
        /// </summary>
        public virtual ICollection<Credential> Credentials { get; set; }
    }
}
