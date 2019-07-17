using System.Collections.Generic;

namespace Neuralm.Domain.Entities.Authentication
{
    /// <summary>
    /// Represents the <see cref="CredentialType"/> class.
    /// </summary>
    public class CredentialType
    {
        /// <summary>
        /// Gets and sets the id.
        /// </summary>
        public int Id { get; set; }

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
