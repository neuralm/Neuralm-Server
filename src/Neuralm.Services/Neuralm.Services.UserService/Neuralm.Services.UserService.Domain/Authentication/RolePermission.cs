using Neuralm.Services.Common.Domain;
using System;

namespace Neuralm.Services.UserService.Domain.Authentication
{
    /// <summary>
    /// Represents the <see cref="RolePermission"/> class.
    /// </summary>
    public class RolePermission : IEntity
    {
        /// <summary>
        /// Gets and sets the id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets and sets the role id.
        /// </summary>
        public Guid RoleId { get; set; }

        /// <summary>
        /// Gets and sets the permission id.
        /// </summary>
        public Guid PermissionId { get; set; }

        /// <summary>
        /// Gets and sets the role.
        /// </summary>
        public virtual Role Role { get; set; }

        /// <summary>
        /// Gets and sets the permission.
        /// </summary>
        public virtual Permission Permission { get; set; }
    }
}
