using Neuralm.Services.Common.Domain;
using System;

namespace Neuralm.Services.UserService.Domain.Authentication
{
    /// <summary>
    /// Represents the <see cref="Credential"/> class.
    /// </summary>
    public class Credential : IEntity
    {
        /// <summary>
        /// Gets and sets the id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets and sets the user id.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets and sets the credential type id.
        /// </summary>
        public Guid CredentialTypeId { get; set; }

        /// <summary>
        /// Gets and sets the identifier.
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// Gets and sets the secret.
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// Gets and sets the extra.
        /// </summary>
        public string Extra { get; set; }

        /// <summary>
        /// Gets and sets the user.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Gets and sets the credential type.
        /// </summary>
        public virtual CredentialType CredentialType { get; set; }
    }
}
