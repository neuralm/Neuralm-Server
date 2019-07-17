namespace Neuralm.Application.Interfaces
{
    /// <summary>
    /// The interface for a hashing algorithm.
    /// </summary>
    public interface IHasher
    {
        bool VerifyHash(string storedHash, string storedSalt, string secret);
        string Hash(string secret, byte[] saltBytes);
    }
}
