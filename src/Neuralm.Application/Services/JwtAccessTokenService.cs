using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security.Claims;
using Neuralm.Application.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Neuralm.Application.Configurations;

namespace Neuralm.Application.Services
{
    public class JwtAccessTokenService : IAccessTokenService
    {
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
        private readonly JwtConfiguration _jwtConfiguration;
        private readonly ConcurrentDictionary<string, SecurityToken> _concurrentTokenDictionary;

        public JwtAccessTokenService(IOptions<JwtConfiguration> jwtConfigurationOptions)
        {
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            _jwtConfiguration = jwtConfigurationOptions.Value;
            _concurrentTokenDictionary = new ConcurrentDictionary<string, SecurityToken>();
        }

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
            _concurrentTokenDictionary.TryAdd(tokenString, token);
            return tokenString;
        }

        public bool ValidateAccessToken(string accessToken)
        {
            throw new NotImplementedException();
        }
    }
}
