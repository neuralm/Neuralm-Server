namespace Neuralm.Services.RegistryService.Application.Configurations
{
    /// <summary>
    /// Represents the <see cref="NeuralmConfiguration"/> class.
    /// </summary>
    public class NeuralmConfiguration
    {
        /// <summary>
        /// Gets and sets the array of services.
        /// </summary>
        public string[] Services { get; set; }

        /// <summary>
        /// Gets and sets the host.
        /// </summary>
        public string Host { get; set; }
        
        /// <summary>
        /// Gets and sets the port.
        /// </summary>
        public int Port { get; set; }
    }
}
