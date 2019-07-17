using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Neuralm.Application.Interfaces
{
    /// <summary>
    /// The interface for AccessTokenService.
    /// </summary>
    public interface IAccessTokenService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims, DateTime expires);
        bool ValidateAccessToken(string accessToken);
    }
}
