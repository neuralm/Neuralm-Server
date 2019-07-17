namespace Neuralm.Domain.Entities.Authentication
{
    /// <summary>
    /// Represents the <see cref="RolePermission"/> class.
    /// </summary>
    public class RolePermission
    {
        /// <summary>
        /// Gets and sets the role id.
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Gets and sets the permission id.
        /// </summary>
        public int PermissionId { get; set; }

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
