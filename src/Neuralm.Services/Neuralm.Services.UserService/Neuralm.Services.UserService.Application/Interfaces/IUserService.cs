using Neuralm.Services.Common.Application.Interfaces;
using Neuralm.Services.UserService.Messages;
using Neuralm.Services.UserService.Messages.Dtos;
using System.Threading.Tasks;

namespace Neuralm.Services.UserService.Application.Interfaces
{
    /// <summary>
    /// Represents the <see cref="IUserService"/> interface.
    /// </summary>
    public interface IUserService : IService<UserDto>
    {
        /// <summary>
        /// Authenticates a user.
        /// </summary>
        /// <param name="authenticateRequest">The authenticate request.</param>
        /// <returns>Returns an awaitable <see cref="Task"/> with parameter type <see cref="AuthenticateResponse"/>.</returns>
        Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest authenticateRequest);

        /// <summary>
        /// Registers a user.
        /// </summary>
        /// <param name="registerRequest">The register request.</param>
        /// <returns>Returns an awaitable <see cref="Task"/> with parameter type <see cref="RegisterResponse"/>.</returns>
        Task<RegisterResponse> RegisterAsync(RegisterRequest registerRequest);
    }
}
