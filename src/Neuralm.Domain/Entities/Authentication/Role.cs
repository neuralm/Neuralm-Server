namespace Neuralm.Domain.Entities.Authentication
{
    /// <summary>
    /// Represents the <see cref="Role"/> class.
    /// </summary>
    public class Role
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
    }
}
