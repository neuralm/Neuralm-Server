using Neuralm.Services.UserService.Application.Interfaces;
using System.Security.Cryptography;

namespace Neuralm.Services.UserService.Application.Cryptography
{
    /// <summary>
    /// Provides a random implementation of the <see cref="ISaltGenerator"/> interface.
    /// </summary>
    public class RandomSaltGenerator : ISaltGenerator
    {
        /// <inheritdoc cref="ISaltGenerator.GenerateSalt()"/>
        public byte[] GenerateSalt()
        {
            byte[] saltAsBytes = new byte[128 / 8];

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                rng.GetBytes(saltAsBytes);

            return saltAsBytes;
        }
    }
}
