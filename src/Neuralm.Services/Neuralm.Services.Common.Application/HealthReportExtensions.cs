using Microsoft.Extensions.Diagnostics.HealthChecks;
using Neuralm.Services.Common.Domain;

namespace Neuralm.Services.Common.Application
{
    /// <summary>
    /// Represents the <see cref="HealthReportExtensions"/> class.
    /// Used to convert a <see cref="HealthReport"/> to a <see cref="ServiceHealthReport"/>.
    /// </summary>
    public static class HealthReportExtensions
    {
        /// <summary>
        /// Converts a <see cref="HealthReport"/> to a <see cref="ServiceHealthReport"/>.
        /// </summary>
        /// <param name="healthReport">The health report.</param>
        /// <returns>Returns the converted <see cref="ServiceHealthReport"/>.</returns>
        public static ServiceHealthReport ToServiceHealthReport(this HealthReport healthReport)
        {
            ServiceHealthReport serviceHealth = new ServiceHealthReport();
            foreach (var (name, entry) in healthReport.Entries)
            {
                HealthCheck healthCheck = new HealthCheck
                {
                    Name = name,
                    Description = entry.Description,
                    Status = entry.Status.ToString(),
                    Duration = new Duration()
                    {
                        Milliseconds = entry.Duration.Milliseconds,
                        Seconds = entry.Duration.Seconds,
                        Ticks = entry.Duration.Ticks
                    }
                };
                serviceHealth.HealthChecks.Add(healthCheck);
            }
            serviceHealth.Status = healthReport.Status.ToString();
            serviceHealth.TotalDuration = new Duration()
            {
                Ticks = healthReport.TotalDuration.Ticks,
                Seconds = healthReport.TotalDuration.Seconds,
                Milliseconds = healthReport.TotalDuration.Milliseconds
            };
            return serviceHealth;
        }
    }
}
