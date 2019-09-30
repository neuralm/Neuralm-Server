using Neuralm.Services.Common.Domain;
using System;
using System.Collections.Generic;

namespace Neuralm.Services.UserService.Domain.Authentication
{
    /// <summary>
    /// Represents the <see cref="CredentialType"/> class.
    /// </summary>
    public class CredentialType : IEntity
    {
        /// <summary>
        /// Gets and sets the id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets and sets the code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets and sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets and sets the position.
        /// </summary>
        public int? Position { get; set; }

        /// <summary>
        /// Gets and sets the collection credentials.
        /// </summary>
        public virtual ICollection<Credential> Credentials { get; set; }
    }
}
