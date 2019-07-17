using System;
using System.Collections.Generic;
using System.Security.Claims;
using Neuralm.Application.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Neuralm.Application.Configurations;
using Microsoft.Extensions.Options;

namespace Neuralm.Application.Services
{
    /// <summary>
    /// The JwtAccessTokenService provides methods to generate access tokens in Jwt format.
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
        /// <param name="claims">The clams</param>
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
        /// <returns><c>true</c> If the Jwt access token is valid; otherwise, <c>false</c>.</returns>
        public bool ValidateAccessToken(string accessToken)
        {
            throw new NotImplementedException();
        }
    }
}
