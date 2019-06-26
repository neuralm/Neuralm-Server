﻿using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Neuralm.Application.Interfaces;
using Neuralm.Application.Messages.Requests;
using Neuralm.Application.Messages.Responses;
using Neuralm.Domain.Entities;
using Neuralm.Domain.Entities.Authentication;
using Neuralm.Domain.Enumerations;

namespace Neuralm.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Credential> _credentialRepository;
        private readonly IRepository<CredentialType> _credentialTypeRepository;
        private readonly IHasher _hasher;
        private readonly ISaltGenerator _saltGenerator;
        private readonly IAccessTokenService _accessTokenService;

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

        public async Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest authenticateRequest)
        {
            if (string.IsNullOrWhiteSpace(authenticateRequest.Username) || string.IsNullOrWhiteSpace(authenticateRequest.Password))
                return new AuthenticateResponse(Guid.NewGuid(), authenticateRequest.Id, error: AuthenticateError.CredentialsAreNullOrEmpty);

            if (!await _credentialTypeRepository.ExistsAsync(ct => CredentialTypeCodePredicate(ct, authenticateRequest.CredentialTypeCode)))
                return new AuthenticateResponse(Guid.NewGuid(), authenticateRequest.Id, error: AuthenticateError.CredentialTypeNotFound);

            CredentialType credentialType = await _credentialTypeRepository.FindSingleByExpressionAsync(ct => CredentialTypeCodePredicate(ct, authenticateRequest.CredentialTypeCode));
            
            if (!await _credentialRepository.ExistsAsync(cred => CredentialPredicate(credentialType, cred, authenticateRequest.Username)))
                return new AuthenticateResponse(Guid.NewGuid(), authenticateRequest.Id, error: AuthenticateError.CredentialNotFound);

            Credential credential = await _credentialRepository.FindSingleByExpressionAsync(cred => CredentialPredicate(credentialType, cred, authenticateRequest.Username));
            if (!_hasher.VerifyHash(credential.Secret, credential.Extra, authenticateRequest.Password))
                return new AuthenticateResponse(Guid.NewGuid(), authenticateRequest.Id, error: AuthenticateError.SecretNotValid);

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, authenticateRequest.Username)
            };
            string accessToken = _accessTokenService.GenerateAccessToken(claims, DateTime.Now.AddHours(2));
            return new AuthenticateResponse(Guid.NewGuid(), authenticateRequest.Id, accessToken, success: true);
        }

        public async Task<RegisterResponse> RegisterAsync(RegisterRequest registerRequest)
        {
            if (string.IsNullOrEmpty(registerRequest.Password))
                return new RegisterResponse(Guid.NewGuid(), registerRequest.Id, RegisterError.CredentialsAreNullOrEmpty);

            if (await _userRepository.ExistsAsync(anyUser => anyUser.Username.Equals(registerRequest.Username, StringComparison.OrdinalIgnoreCase)))
                return new RegisterResponse(Guid.NewGuid(), registerRequest.Id, RegisterError.UsernameAlreadyExists);

            if (!await _credentialTypeRepository.ExistsAsync(ct => CredentialTypeCodePredicate(ct, registerRequest.CredentialTypeCode)))
                return new RegisterResponse(Guid.NewGuid(), registerRequest.Id, RegisterError.CredentialTypeNotFound);

            User user = new User
            {
                Username = registerRequest.Username
            };
            if (!await _userRepository.CreateAsync(user))
                return new RegisterResponse(Guid.NewGuid(), registerRequest.Id, RegisterError.PersistenceError);

            CredentialType credentialType = await _credentialTypeRepository.FindSingleByExpressionAsync(ct => CredentialTypeCodePredicate(ct, registerRequest.CredentialTypeCode));
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
            return new RegisterResponse(Guid.NewGuid(), registerRequest.Id, success: success);
        }

        private static bool CredentialPredicate(CredentialType credentialType, Credential credential, string username) 
            => credential.CredentialTypeId.Equals(credentialType.Id) && string.Equals(credential.Identifier, username, StringComparison.OrdinalIgnoreCase);

        private static bool CredentialTypeCodePredicate(CredentialType credentialType, string credentialTypeCode) 
            => string.Equals(credentialType.Code, credentialTypeCode, StringComparison.OrdinalIgnoreCase);
    }
}