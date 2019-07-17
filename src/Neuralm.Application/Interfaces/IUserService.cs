using Neuralm.Application.Messages.Requests;
using System.Threading.Tasks;
using Neuralm.Application.Messages.Responses;

namespace Neuralm.Application.Interfaces
{
    /// <summary>
    /// The interface for the user service.
    /// </summary>
    public interface IUserService : IService
    {
        Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest authenticateRequest);
        Task<RegisterResponse> RegisterAsync(RegisterRequest registerRequest);
    }
}
