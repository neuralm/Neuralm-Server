namespace Neuralm.Domain.Entities.Authentication
{
    /// <summary>
    /// Represents the <see cref="UserRole"/> class.
    /// </summary>
    public class UserRole
    {
        /// <summary>
        /// Gets and sets the user id.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets and sets the role id.
        /// </summary>
        public int RoleId { get; set; }

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
