using System.Threading.Tasks;
using Neuralm.Application.Messages.Requests;
using Neuralm.Application.Messages.Responses;

namespace Neuralm.Application.Interfaces
{
    public interface ITrainingRoomService : IService
    {
        Task<CreateTrainingRoomResponse> CreateTrainingRoomAsync(CreateTrainingRoomRequest createTrainingRoomRequest);
        Task<DisableTrainingRoomResponse> DisableTrainingRoomAsync(DisableTrainingRoomRequest disableTrainingRoomRequest);
        Task<EnableTrainingRoomResponse> EnableTrainingRoomAsync(EnableTrainingRoomRequest enableTrainingRoomRequest);
        Task<AuthorizeUserForTrainingRoomResponse> AuthorizeUserForTrainingRoomAsync(AuthorizeUserForTrainingRoomRequest authorizeUserForRoomRequest);
        Task<DeauthorizeUserForTrainingRoomResponse> DeauthorizeUserForTrainingRoomAsync(DeauthorizeUserForTrainingRoomRequest authorizeUserForRoomRequest);
        Task<UpdateScoresResponse> UpdateScoresAsync(UpdateScoresRequest updateScoresRequest);
        Task<GetOrganismsResponse> GetOrganismsAsync(GetOrganismsRequest getOrganismsRequest);
        Task<GetBestOrganismsResponse> GetBestOrganismsAsync(GetBestOrganismsRequest getBestOrganismsRequest);
        Task<StartTrainingSessionResponse> StartTrainingSessionAsync(StartTrainingSessionRequest startTrainingSessionRequest);
        Task<EndTrainingSessionResponse> EndTrainingSessionAsync(EndTrainingSessionRequest endTrainingSessionRequest);
        Task<GetGenerationStatusResponse> GetGenerationStatusAsync(GetGenerationStatusRequest getGenerationStatusRequest);
    }
}
