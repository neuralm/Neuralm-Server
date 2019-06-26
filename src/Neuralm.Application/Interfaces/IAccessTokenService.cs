using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Neuralm.Application.Interfaces
{
    public interface IAccessTokenService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims, DateTime expires);
        bool ValidateAccessToken(string accessToken);
    }
}
