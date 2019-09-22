using Neuralm.Services.Common.Domain;
using System;

namespace Neuralm.Services.UserService.Domain.Authentication
{
    /// <summary>
    /// Represents the <see cref="UserRole"/> class.
    /// </summary>
    public class UserRole : IEntity
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
        /// Gets and sets the role id.
        /// </summary>
        public Guid RoleId { get; set; }

        /// <summary>
        /// Gets and sets the user.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Gets and sets the role.
        /// </summary>
        public virtual Role Role { get; set; }

    }
}
