using System.Configuration;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;

namespace Neuralm.Application.Configurations
{
    /// <summary>
    /// The server configuration class which holds the port the server is hosted on.
    /// </summary>
    public class ServerConfiguration
    {
        [ConfigurationProperty("ClientPort", IsRequired = true)]
        [IntegerValidator(MinValue = 0, MaxValue = 9999, ExcludeRange = false)]
        public int ClientPort { get; set; }

        [ConfigurationProperty("RestPort", IsRequired = true)]
        [IntegerValidator(MinValue = 0, MaxValue = 9999, ExcludeRange = false)]
        public int RestPort { get; set; }

        [ConfigurationProperty("Host", IsRequired = true)]
        [StringValidator(MinLength = 6)]
        public string Host { get; set; }

        [IgnoreDataMember]
        public X509Certificate Certificate { get; set; }
    }
}
