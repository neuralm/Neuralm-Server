namespace Neuralm.Application.Messages.Requests
{
    /// <summary>
    /// Represents the <see cref="RegisterRequest"/> class.
    /// </summary>
    public class RegisterRequest : Request
    {
        /// <summary>
        /// Gets the username.
        /// </summary>
        public string Username { get; }

        /// <summary>
        /// Gets the password.
        /// </summary>
        public string Password { get; }

        /// <summary>
        /// Gets the credential type code.
        /// </summary>
        public string CredentialTypeCode { get; }

        /// <summary>
        /// Initializes an instance of the <see cref="RegisterRequest"/> class.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="credentialTypeCode">The credential type code.</param>
        public RegisterRequest(string username, string password, string credentialTypeCode)
        {
            Username = username;
            Password = password;
            CredentialTypeCode = credentialTypeCode;
        }
    }
}
