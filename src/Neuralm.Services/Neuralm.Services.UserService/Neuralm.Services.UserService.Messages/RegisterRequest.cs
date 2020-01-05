using Neuralm.Services.Common.Messages.Abstractions;
using System.ComponentModel.DataAnnotations;
using Neuralm.Services.Common.Messages;

namespace Neuralm.Services.UserService.Messages
{
    /// <summary>
    /// Represents the <see cref="RegisterRequest"/> class.
    /// </summary>
    [Message("Post", "/register", typeof(RegisterResponse))]
    public class RegisterRequest : Request
    {
        /// <summary>
        /// Gets the username.
        /// </summary>
        [Required, StringLength(45, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 3)]
        public string Username { get; set; }

        /// <summary>
        /// Gets the password.
        /// </summary>
        [Required, RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).{9,64}$", ErrorMessage = "Password must contain 1 Uppercase, 1 Lowercase, 1 Number, and a minimum length of 9."), DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// Gets the credential type code.
        /// </summary>
        [Required]
        public string CredentialTypeCode { get; set; }
    }
}
