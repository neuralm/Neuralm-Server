﻿using System.Collections.Generic;
using System.Security.Claims;

namespace Neuralm.Services.Common.Application.Interfaces
{
    /// <summary>
    /// Represents the <see cref="IAccessTokenService"/> interface.
    /// </summary>
    public interface IAccessTokenService
    {
        /// <summary>
        /// Generates an access token as string.
        /// </summary>
        /// <param name="claims">The claims.</param>
        /// <returns>Returns a generated access token as string.</returns>
        string GenerateAccessToken(IEnumerable<Claim> claims);

        /// <summary>
        /// Validates an access token.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="claimsPrincipal">The claims principal.</param>
        /// <returns>Returns <c>true</c> if the access token is valid; otherwise, <c>false</c>.</returns>
        bool ValidateAccessToken(string accessToken, out ClaimsPrincipal claimsPrincipal);
    }
}
