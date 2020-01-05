using Neuralm.Services.Common.Messages.Abstractions;
using System.ComponentModel.DataAnnotations;
using Neuralm.Services.Common.Messages;

namespace Neuralm.Services.UserService.Messages
{
    /// <summary>
    /// Represents the <see cref="AuthenticateRequest"/> class.
    /// </summary>
    [Message("Post", "/authenticate", typeof(AuthenticateResponse))]
    public class AuthenticateRequest : Request
    {
        /// <summary>
        /// Gets the username.
        /// </summary>
        [Required, StringLength(45, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 3)]
        public string Username { get; set; }

        /// <summary>
        /// Gets the password.
        /// </summary>
        [Required, StringLength(64, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 9), DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// Gets the credential type code.
        /// </summary>
        [Required]
        public string CredentialTypeCode { get; set; }
    }
}
