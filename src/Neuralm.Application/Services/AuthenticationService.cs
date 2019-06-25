using System.Threading.Tasks;
using Neuralm.Application.Interfaces;
using Neuralm.Application.Messages.Requests;
using Neuralm.Application.Messages.Responses;
using Neuralm.Domain.Entities;

namespace Neuralm.Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IHasher _hasher;
        private readonly ISaltGenerator _saltGenerator;

        public AuthenticationService(
            IRepository<User> userRepository,
            IHasher hasher, 
            ISaltGenerator saltGenerator)
        {
            _userRepository = userRepository;
            _hasher = hasher;
            _saltGenerator = saltGenerator;
        }

        public Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
        {
            throw new System.NotImplementedException();
        }

        public Task<RegisterResponse> RegisterAsync(RegisterRequest registerRequest)
        {
            throw new System.NotImplementedException();
        }
    }
}