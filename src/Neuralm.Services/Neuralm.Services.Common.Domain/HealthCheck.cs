namespace Neuralm.Services.Common.Domain
{
    /// <summary>
    /// Represents the <see cref="HealthCheck"/> class.
    /// </summary>
    public class HealthCheck
    {
        /// <summary>
        /// Gets and sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets and sets the status.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets and sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets and sets the duration.
        /// </summary>
        public Duration Duration { get; set; }
    }
}
