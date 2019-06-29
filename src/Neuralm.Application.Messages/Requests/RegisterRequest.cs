namespace Neuralm.Application.Messages.Requests
{
    public class RegisterRequest : Request
    {
        public string Username { get; }
        public string Password { get; }
        public string CredentialTypeCode { get; }

        public RegisterRequest(string username, string password, string credentialTypeCode)
        {
            Username = username;
            Password = password;
            CredentialTypeCode = credentialTypeCode;
        }
    }
}
