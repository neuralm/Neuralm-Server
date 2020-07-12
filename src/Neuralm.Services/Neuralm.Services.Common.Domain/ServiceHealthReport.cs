using System.Collections.Generic;

namespace Neuralm.Services.Common.Domain
{
    /// <summary>
    /// Represents the <see cref="ServiceHealthReport"/> class.
    /// </summary>
    public class ServiceHealthReport
    {
        /// <summary>
        /// Gets and sets the status.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets and sets the list of health checks.
        /// </summary>
        public List<HealthCheck> HealthChecks { get; set; } = new List<HealthCheck>();

        /// <summary>
        /// Gets and sets the total duration.
        /// </summary>
        public Duration TotalDuration { get; set; }
    }
}
