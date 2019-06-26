using System;
using System.Collections.Generic;
using System.Text;

namespace Neuralm.Application.Interfaces
{
    public interface IHasher
    {
        bool VerifyHash(string storedHash, string storedSalt, string secret);
        string Hash(string secret, byte[] saltBytes);
    }
}
