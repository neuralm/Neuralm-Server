using System.Threading.Tasks;
using Neuralm.Application.Messages.Requests;
using Neuralm.Application.Messages.Responses;

namespace Neuralm.Application.Interfaces
{
    public interface IRoomService
    {
        Task<CreateRoomResponse> CreateRoomAsync(CreateRoomRequest createRoomRequest);
        // AuthorizeUserForRoomAsync
    }
}
