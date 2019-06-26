using Neuralm.Application.Messages.Requests;
using System.Threading.Tasks;
using Neuralm.Application.Messages.Responses;

namespace Neuralm.Application.Interfaces
{
    public interface IUserService
    {
        Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest authenticateRequest);
        Task<RegisterResponse> RegisterAsync(RegisterRequest registerRequest);
    }
}
