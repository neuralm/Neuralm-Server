using AutoMapper;
using Neuralm.Services.Common.Application.Interfaces;
using Neuralm.Services.Common.Exceptions;
using Neuralm.Services.UserService.Application.Dtos;
using Neuralm.Services.UserService.Application.Interfaces;
using Neuralm.Services.UserService.Application.Models;
using Neuralm.Services.UserService.Domain;
using Neuralm.Services.UserService.Domain.Authentication;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Neuralm.Services.UserService.Application.Services
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
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes an instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="credentialRepository">The credential repository.</param>
        /// <param name="credentialTypeRepository">The credential type repository.</param>
        /// <param name="hasher">The hasher.</param>
        /// <param name="saltGenerator">The salt generator.</param>
        /// <param name="accessTokenService">The access token service.</param>
        /// <param name="mapper">The mapper.</param>
        public UserService(
            IRepository<User> userRepository,
            IRepository<Credential> credentialRepository,
            IRepository<CredentialType> credentialTypeRepository,
            IHasher hasher,
            ISaltGenerator saltGenerator,
            IAccessTokenService accessTokenService,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _credentialRepository = credentialRepository;
            _credentialTypeRepository = credentialTypeRepository;
            _hasher = hasher;
            _saltGenerator = saltGenerator;
            _accessTokenService = accessTokenService;
            _mapper = mapper;
        }

        /// <inheritdoc cref="IUserService.AuthenticateAsync(AuthenticateRequest)"/>
        public async Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest authenticateRequest)
        {
            if (string.IsNullOrWhiteSpace(authenticateRequest.Username) || string.IsNullOrWhiteSpace(authenticateRequest.Password))
                return new AuthenticateResponse(authenticateRequest.Id, Guid.Empty, message: "Credentials are null or empty.");

            if (!await _credentialTypeRepository.ExistsAsync(ct => ct.Code == authenticateRequest.CredentialTypeCode))
                return new AuthenticateResponse(authenticateRequest.Id, Guid.Empty, message: "CredentialType not found.");

            CredentialType credentialType = await _credentialTypeRepository.FindSingleOrDefaultAsync(ct => ct.Code == authenticateRequest.CredentialTypeCode);
            if (!await _credentialRepository.ExistsAsync(cred => cred.CredentialTypeId == credentialType.Id && cred.Identifier == authenticateRequest.Username))
                return new AuthenticateResponse(authenticateRequest.Id, Guid.Empty, message: "Credentials not found.");

            Credential credential = await _credentialRepository.FindSingleOrDefaultAsync(cred => cred.CredentialTypeId == credentialType.Id && cred.Identifier == authenticateRequest.Username);
            if (!_hasher.VerifyHash(credential.Secret, credential.Extra, authenticateRequest.Password))
                return new AuthenticateResponse(authenticateRequest.Id, Guid.Empty, message: "Secret not valid.");

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, authenticateRequest.Username),
                new Claim("Authorized", "Logged in")
            };
            User user = await _userRepository.FindSingleOrDefaultAsync(usr => usr.Username == authenticateRequest.Username);
            string accessToken = _accessTokenService.GenerateAccessToken(claims, DateTime.Now.AddHours(2));
            return new AuthenticateResponse(authenticateRequest.Id, user.Id, accessToken, success: true);
        }

        /// <inheritdoc cref="IUserService.RegisterAsync(RegisterRequest)"/>
        public async Task<RegisterResponse> RegisterAsync(RegisterRequest registerRequest)
        {
            if (string.IsNullOrEmpty(registerRequest.Password))
                return new RegisterResponse(registerRequest.Id, message: "Credentials are null or empty.");

            if (await _userRepository.ExistsAsync(anyUser => anyUser.Username == registerRequest.Username))
                return new RegisterResponse(registerRequest.Id, message: "Username already exists.");

            if (!await _credentialTypeRepository.ExistsAsync(ct => ct.Code == registerRequest.CredentialTypeCode))
                return new RegisterResponse(registerRequest.Id, message: "CredentialType not found.");

            User user = new User
            {
                Username = registerRequest.Username,
                TimestampCreated = DateTime.UtcNow
            };
            (bool createdSuccess, _) = await _userRepository.CreateAsync(user);
            if (!createdSuccess)
                return new RegisterResponse(registerRequest.Id, "Failed to persist data.");

            CredentialType credentialType = await _credentialTypeRepository.FindSingleOrDefaultAsync(ct => ct.Code == registerRequest.CredentialTypeCode);
            byte[] salt = _saltGenerator.GenerateSalt();
            Credential credential = new Credential
            {
                UserId = user.Id,
                CredentialTypeId = credentialType.Id,
                Identifier = registerRequest.Username,
                Secret = _hasher.Hash(registerRequest.Password, salt),
                Extra = Convert.ToBase64String(salt)
            };

            (bool success, _) = await _credentialRepository.CreateAsync(credential);
            return new RegisterResponse(registerRequest.Id, success: success);
        }

        /// <inheritdoc cref="IService{TEntity}.FindSingleOrDefaultAsync(Guid)"/>
        public Task<UserDto> FindSingleOrDefaultAsync(Guid id)
        {
            return _userRepository.FindSingleOrDefaultAsync(user => user.Id.Equals(id))
                .ContinueWith(user => user.Equals(default) ? default : _mapper.Map<UserDto>(user.Result));
        }

        /// <inheritdoc cref="IService{TEntity}.GetAllAsync()"/>
        public Task<IEnumerable<UserDto>> GetAllAsync()
        {
            return _userRepository.GetAllAsync()
                .ContinueWith(users => _mapper.Map<IEnumerable<UserDto>>(users.Result));
        }

        /// <inheritdoc cref="IService{TEntity}.DeleteAsync(TEntity)"/>
        public async Task<(bool success, bool found)> DeleteAsync(UserDto entity)
        {
            User user = _mapper.Map<User>(entity);
            bool found = await _userRepository.ExistsAsync(u => u.Id.Equals(user.Id));
            return !found ? (false, false) : (await _userRepository.DeleteAsync(user), true);
        }

        /// <inheritdoc cref="IService{TEntity}.UpdateAsync(TEntity)"/>
        public Task<(bool success, Guid id, bool updated)> UpdateAsync(UserDto entity)
        {
            User user = _mapper.Map<User>(entity);
            return _userRepository.UpdateAsync(user);
        }

        /// <inheritdoc cref="IService{TEntity}.CreateAsync(TEntity)"/>
        public Task<(bool success, Guid id)> CreateAsync(UserDto entity)
        {
            throw new CreatingEntityFailedException("When creating a user use the RegisterAsync method.");
        }
    }
}
