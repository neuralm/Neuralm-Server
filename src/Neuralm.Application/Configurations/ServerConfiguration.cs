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
        [ConfigurationProperty("Port", IsRequired = true)]
        [IntegerValidator(MinValue = 0, MaxValue = 8080, ExcludeRange = false)]
        public int Port { get; set; }

        [ConfigurationProperty("CertificateName", IsRequired = true)]
        [RegexStringValidator("^([a-zA-Z0-9-_/]*).cer$")]
        public string CertificateName { get; set; }

        [ConfigurationProperty("Password", IsRequired = true)]
        public string Password { get; set; }

        [IgnoreDataMember]
        public X509Certificate Certificate { get; set; }
    }
}
