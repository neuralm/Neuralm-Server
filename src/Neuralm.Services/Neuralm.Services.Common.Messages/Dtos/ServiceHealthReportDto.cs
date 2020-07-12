using System.Collections.Generic;

namespace Neuralm.Services.Common.Messages.Dtos
{
    /// <summary>
    /// Represents the <see cref="ServiceHealthReportDto"/> of the ServiceHealthReport class.
    /// </summary>
    public class ServiceHealthReportDto
    {
        /// <summary>
        /// Gets and sets the status.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets and sets the list of health checks.
        /// </summary>
        public List<HealthCheckDto> HealthChecks { get; set; } = new List<HealthCheckDto>();

        /// <summary>
        /// Gets and sets the total duration.
        /// </summary>
        public DurationDto TotalDuration { get; set; }
    }
}
