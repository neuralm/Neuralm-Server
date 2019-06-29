using System.Threading.Tasks;
using Neuralm.Application.Messages.Requests;
using Neuralm.Application.Messages.Responses;

namespace Neuralm.Application.Interfaces
{
    public interface ITrainingRoomService : IService
    {
        Task<CreateTrainingRoomResponse> CreateTrainingRoomAsync(CreateTrainingRoomRequest createTrainingRoomRequest);
        // Disable (Delete after not re-instating the training room after X amount of time)
        Task<DisableTrainingRoomResponse> DisableTrainingRoomAsync(DisableTrainingRoomRequest disableTrainingRoomRequest);

        // Reenable
        // AuthorizeUserForRoom
        // DeauthorizeUserForRoom
        // UpdateSettings
        // GetOrganisms
        // UpdateScores
        // GetBestOrganisms
        // StartTrainingSession
        // EndTrainingSession
        // GetGenerationStatus
    }
}
