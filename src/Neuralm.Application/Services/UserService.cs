using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Neuralm.Application.Interfaces;
using Neuralm.Application.Messages.Requests;
using Neuralm.Application.Messages.Responses;
using Neuralm.Domain.Entities;
using Neuralm.Domain.Entities.Authentication;

namespace Neuralm.Application.Services
{
    /// <summary>
    /// Represents the <see cref="UserService"/> class.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Credential> _credentialRepository;
        private readonly IRepository<CredentialType> _credentialTypeRepository;
        private readonly IHasher _hasher;
        private readonly ISaltGenerator _saltGenerator;
        private readonly IAccessTokenService _accessTokenService;

        /// <summary>
        /// Initializes an instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="credentialRepository">The credential repository.</param>
        /// <param name="credentialTypeRepository">The credential type repository.</param>
        /// <param name="hasher">The hasher.</param>
        /// <param name="saltGenerator">The salt generator.</param>
        /// <param name="accessTokenService">The access token service.</param>
        public UserService(
            IRepository<User> userRepository,
            IRepository<Credential> credentialRepository,
            IRepository<CredentialType> credentialTypeRepository,
            IHasher hasher, 
            ISaltGenerator saltGenerator,
            IAccessTokenService accessTokenService)
        {
            _userRepository = userRepository;
            _credentialRepository = credentialRepository;
            _credentialTypeRepository = credentialTypeRepository;
            _hasher = hasher;
            _saltGenerator = saltGenerator;
            _accessTokenService = accessTokenService;
        }

        /// <inheritdoc cref="IUserService.AuthenticateAsync(AuthenticateRequest)"/>
        public async Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest authenticateRequest)
        {
            if (string.IsNullOrWhiteSpace(authenticateRequest.Username) || string.IsNullOrWhiteSpace(authenticateRequest.Password))
                return new AuthenticateResponse(authenticateRequest.Id, Guid.Empty, message: "Credentials are null or empty.");

            if (!await _credentialTypeRepository.ExistsAsync(ct => CredentialTypeCodePredicate(ct, authenticateRequest.CredentialTypeCode)))
                return new AuthenticateResponse(authenticateRequest.Id, Guid.Empty, message: "CredentialType not found.");

            CredentialType credentialType = await _credentialTypeRepository.FindSingleOrDefaultAsync(ct => CredentialTypeCodePredicate(ct, authenticateRequest.CredentialTypeCode));
            if (!await _credentialRepository.ExistsAsync(cred => CredentialPredicate(credentialType, cred, authenticateRequest.Username)))
                return new AuthenticateResponse(authenticateRequest.Id, Guid.Empty, message: "Credentials not found.");

            Credential credential = await _credentialRepository.FindSingleOrDefaultAsync(cred => CredentialPredicate(credentialType, cred, authenticateRequest.Username));
            if (!_hasher.VerifyHash(credential.Secret, credential.Extra, authenticateRequest.Password))
                return new AuthenticateResponse(authenticateRequest.Id, Guid.Empty, message: "Secret not valid.");

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, authenticateRequest.Username)
            };
            User user = await _userRepository.FindSingleOrDefaultAsync(usr => string.Equals(usr.Username, authenticateRequest.Username, StringComparison.OrdinalIgnoreCase));
            string accessToken = _accessTokenService.GenerateAccessToken(claims, DateTime.Now.AddHours(2));
            return new AuthenticateResponse(authenticateRequest.Id, user.Id, accessToken, success: true);
        }

        /// <inheritdoc cref="IUserService.RegisterAsync(RegisterRequest)"/>
        public async Task<RegisterResponse> RegisterAsync(RegisterRequest registerRequest)
        {
            if (string.IsNullOrEmpty(registerRequest.Password))
                return new RegisterResponse(registerRequest.Id, message: "Credentials are null or empty.");

            if (await _userRepository.ExistsAsync(anyUser => anyUser.Username.Equals(registerRequest.Username, StringComparison.OrdinalIgnoreCase)))
                return new RegisterResponse(registerRequest.Id, message: "Username already exists.");

            if (!await _credentialTypeRepository.ExistsAsync(ct => CredentialTypeCodePredicate(ct, registerRequest.CredentialTypeCode)))
                return new RegisterResponse(registerRequest.Id, message: "CredentialType not found.");

            User user = new User
            {
                Username = registerRequest.Username
            };
            if (!await _userRepository.CreateAsync(user))
                return new RegisterResponse(registerRequest.Id, "Failed to persist data.");

            CredentialType credentialType = await _credentialTypeRepository.FindSingleOrDefaultAsync(ct => CredentialTypeCodePredicate(ct, registerRequest.CredentialTypeCode));
            byte[] salt = _saltGenerator.GenerateSalt();
            Credential credential = new Credential
            {
                UserId = user.Id,
                CredentialTypeId = credentialType.Id,
                Identifier = registerRequest.Username,
                Secret = _hasher.Hash(registerRequest.Password, salt),
                Extra = Convert.ToBase64String(salt)
            };

            bool success = await _credentialRepository.CreateAsync(credential);
            return new RegisterResponse(registerRequest.Id, success: success);
        }

        private static bool CredentialPredicate(CredentialType credentialType, Credential credential, string username) 
            => credential.CredentialTypeId.Equals(credentialType.Id) && string.Equals(credential.Identifier, username, StringComparison.OrdinalIgnoreCase);
        private static bool CredentialTypeCodePredicate(CredentialType credentialType, string credentialTypeCode) 
            => string.Equals(credentialType.Code, credentialTypeCode, StringComparison.OrdinalIgnoreCase);
    }
}