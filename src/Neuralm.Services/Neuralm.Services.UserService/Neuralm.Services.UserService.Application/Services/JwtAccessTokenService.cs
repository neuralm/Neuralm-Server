using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Neuralm.Services.Common.Configurations;
using Neuralm.Services.UserService.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Neuralm.Services.UserService.Application.Services
{
    /// <summary>
    /// Represents the <see cref="JwtAccessTokenService"/> class, provides methods to generate access tokens in Jwt format.
    /// RFC 7519: https://tools.ietf.org/html/rfc7519
    /// </summary>
    public class JwtAccessTokenService : IAccessTokenService
    {
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
        private readonly JwtConfiguration _jwtConfiguration;

        /// <summary>
        /// Initializes an instance of the <see cref="JwtAccessTokenService"/> class.
        /// </summary>
        /// <param name="jwtConfigurationOptions">The options.</param>
        public JwtAccessTokenService(IOptions<JwtConfiguration> jwtConfigurationOptions)
        {
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            _jwtConfiguration = jwtConfigurationOptions.Value;
        }

        /// <summary>
        /// Generates a Jwt access token using the provided claims and expiry date.
        /// </summary>
        /// <param name="claims">The claims.</param>
        /// <param name="expires">Th expiry date.</param>
        /// <returns>Returns the Jwt access token as string.</returns>
        public string GenerateAccessToken(IEnumerable<Claim> claims, DateTime expires)
        {
            byte[] key = Encoding.ASCII.GetBytes(_jwtConfiguration.Secret);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expires,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken token = _jwtSecurityTokenHandler.CreateToken(tokenDescriptor);
            string tokenString = _jwtSecurityTokenHandler.WriteToken(token);
            return tokenString;
        }

        /// <summary>
        /// Validates the Jwt access token.
        /// </summary>
        /// <param name="accessToken">The Jwt access token as string.</param>
        /// <param name="claimsPrincipal">The claims principal.</param>
        /// <returns>Returns <c>true</c> if the Jwt access token is valid; otherwise, <c>false</c>.</returns>
        public bool ValidateAccessToken(string accessToken, out ClaimsPrincipal claimsPrincipal)
        {
            TokenValidationParameters tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtConfiguration.Secret))
            };
            try
            {
                claimsPrincipal = _jwtSecurityTokenHandler.ValidateToken(accessToken, tokenValidationParameters, out SecurityToken validatedToken);
                return true;
            }
            catch (Exception)
            {
                claimsPrincipal = null;
                return false;
            }
        }
    }
}
