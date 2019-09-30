using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;

namespace Neuralm.Services.MessageQueue.Application.Configurations
{
    /// <summary>
    /// Represents the <see cref="MessageQueueConfiguration"/> class.
    /// </summary>
    public class MessageQueueConfiguration
    {
        /// <summary>
        /// Gets and sets the host.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Gets and sets the port.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Gets and sets the certificate.
        /// </summary>
        [JsonIgnore]
        public X509Certificate2 Certificate { get; set; }
    }
}
