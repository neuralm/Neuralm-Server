using System.Text;
using System.Text.Json.Serialization;

namespace Neuralm.Services.Common.Configurations
{
    /// <summary>
    /// Represents the <see cref="JwtConfiguration"/> class.
    /// </summary>
    public class JwtConfiguration
    {
        /// <summary>
        /// Gets and sets the jwt secret.
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// Gets the secret as bytes.
        /// </summary>
        [JsonIgnore]
        public byte[] SecretBytes => Encoding.ASCII.GetBytes(Secret);

        /// <summary>
        /// Gets and sets the issuer.
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Gets and sets the audience.
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// Gets and sets the access expiration.
        /// </summary>
        public int AccessExpiration { get; set; }

        /// <summary>
        /// Gets and sets the refresh expiration.
        /// </summary>
        public int RefreshExpiration { get; set; }
    }
}
