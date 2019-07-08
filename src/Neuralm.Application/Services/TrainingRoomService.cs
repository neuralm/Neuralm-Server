using System;
using System.Threading.Tasks;
using Neuralm.Application.Interfaces;
using Neuralm.Application.Messages.Requests;
using Neuralm.Application.Messages.Responses;
using Neuralm.Domain.Entities.NEAT;

namespace Neuralm.Application.Services
{
    public class TrainingRoomService : ITrainingRoomService
    {
        private readonly IRepository<TrainingRoom> _trainingRoomRepository;

        public TrainingRoomService(IRepository<TrainingRoom> trainingRoomRepository)
        {
            _trainingRoomRepository = trainingRoomRepository;
        }

        public async Task<CreateTrainingRoomResponse> CreateTrainingRoomAsync(CreateTrainingRoomRequest createTrainingRoomRequest)
        {
            if (createTrainingRoomRequest.OwnerId < 1 || string.IsNullOrWhiteSpace(createTrainingRoomRequest.TrainingRoomName))
                return new CreateTrainingRoomResponse(createTrainingRoomRequest.Id, false);
            // 
            return null;
        }
        public Task<DisableTrainingRoomResponse> DisableTrainingRoomAsync(DisableTrainingRoomRequest disableTrainingRoomRequest)
        {
            throw new NotImplementedException();
        }
        public Task<EnableTrainingRoomResponse> EnableTrainingRoomAsync(EnableTrainingRoomRequest enableTrainingRoomRequest)
        {
            throw new NotImplementedException();
        }
        public Task<AuthorizeUserForTrainingRoomResponse> AuthorizeUserForTrainingRoomAsync(AuthorizeUserForTrainingRoomRequest authorizeUserForRoomRequest)
        {
            throw new NotImplementedException();
        }
        public Task<DeauthorizeUserForTrainingRoomResponse> DeauthorizeUserForTrainingRoomAsync(DeauthorizeUserForTrainingRoomRequest authorizeUserForRoomRequest)
        {
            throw new NotImplementedException();
        }
        public Task<UpdateScoresResponse> UpdateScoresAsync(UpdateScoresRequest updateScoresRequest)
        {
            throw new NotImplementedException();
        }
        public Task<GetOrganismsResponse> GetOrganismsAsync(GetOrganismsRequest getOrganismsRequest)
        {
            throw new NotImplementedException();
        }
        public Task<GetBestOrganismsResponse> GetBestOrganismsAsync(GetBestOrganismsRequest getBestOrganismsRequest)
        {
            throw new NotImplementedException();
        }
        public Task<StartTrainingSessionResponse> StartTrainingSessionAsync(StartTrainingSessionRequest startTrainingSessionRequest)
        {
            throw new NotImplementedException();
        }
        public Task<EndTrainingSessionResponse> EndTrainingSessionAsync(EndTrainingSessionRequest endTrainingSessionRequest)
        {
            throw new NotImplementedException();
        }
        public Task<GetGenerationStatusResponse> GetGenerationStatusAsync(GetGenerationStatusRequest getGenerationStatusRequest)
        {
            throw new NotImplementedException();
        }
    }
}
