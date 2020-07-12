namespace Neuralm.Services.Common.Messages.Dtos
{
    /// <summary>
    /// Represents the <see cref="HealthCheckDto"/> of the HealthCheck class.
    /// </summary>
    public class HealthCheckDto
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public DurationDto Duration { get; set; }
    }
}
