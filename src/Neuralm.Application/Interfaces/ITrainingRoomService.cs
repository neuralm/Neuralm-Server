using System.Threading.Tasks;
using Neuralm.Application.Messages.Requests;
using Neuralm.Application.Messages.Responses;

namespace Neuralm.Application.Interfaces
{
    public interface ITrainingRoomService : IService
    {
        Task<CreateTrainingRoomResponse> CreateTrainingRoomAsync(CreateTrainingRoomRequest createTrainingRoomRequest);
        Task<StartTrainingSessionResponse> StartTrainingSessionAsync(StartTrainingSessionRequest startTrainingSessionRequest);
        Task<EndTrainingSessionResponse> EndTrainingSessionAsync(EndTrainingSessionRequest endTrainingSessionRequest);
        Task<GetEnabledTrainingRoomsResponse> GetEnabledTrainingRoomsAsync(GetEnabledTrainingRoomsRequest getEnabledTrainingRoomsRequest);
    }
}
