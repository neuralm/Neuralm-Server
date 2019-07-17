using System.Security.Cryptography;
using Neuralm.Application.Interfaces;

namespace Neuralm.Application.Cryptography
{
    /// <summary>
    /// Provides a random implementation of the <see cref="ISaltGenerator"/> interface.
    /// </summary>
    public class RandomSaltGenerator : ISaltGenerator
    {
        public byte[] GenerateSalt()
        {
            byte[] saltAsBytes = new byte[128 / 8];

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                rng.GetBytes(saltAsBytes);

            return saltAsBytes;
        }
    }
}
