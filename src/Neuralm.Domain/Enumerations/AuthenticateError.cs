namespace Neuralm.Domain.Enumerations
{
    public enum AuthenticateError
    {
        CredentialsAreNullOrEmpty,
        CredentialTypeNotFound,
        CredentialNotFound,
        SecretNotValid,
        None
    }
}
