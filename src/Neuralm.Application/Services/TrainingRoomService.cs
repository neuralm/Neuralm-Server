using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Neuralm.Application.Interfaces;
using Neuralm.Application.Messages.Dtos;
using Neuralm.Application.Messages.Requests;
using Neuralm.Application.Messages.Responses;
using Neuralm.Domain.Entities;
using Neuralm.Domain.Entities.NEAT;

namespace Neuralm.Application.Services
{
    public class TrainingRoomService : ITrainingRoomService
    {
        private readonly IRepository<TrainingRoom> _trainingRoomRepository;
        private readonly IRepository<User> _userRepository;

        public TrainingRoomService(
            IRepository<TrainingRoom> trainingRoomRepository,
            IRepository<User> userRepository)
        {
            _trainingRoomRepository = trainingRoomRepository;
            _userRepository = userRepository;
        }

        public async Task<CreateTrainingRoomResponse> CreateTrainingRoomAsync(CreateTrainingRoomRequest createTrainingRoomRequest)
        {
            if (createTrainingRoomRequest.OwnerId.Equals(Guid.Empty))
                return new CreateTrainingRoomResponse(createTrainingRoomRequest.Id, Guid.Empty, "The ownerId must not be an empty guid.");

            if (string.IsNullOrWhiteSpace(createTrainingRoomRequest.TrainingRoomName))
                return new CreateTrainingRoomResponse(createTrainingRoomRequest.Id, Guid.Empty, "The training room name cannot be null or be empty");

            if (await _trainingRoomRepository.ExistsAsync(tr => tr.Name.Equals(createTrainingRoomRequest.TrainingRoomName, StringComparison.CurrentCultureIgnoreCase)))
                return new CreateTrainingRoomResponse(createTrainingRoomRequest.Id, Guid.Empty, "A training room with the requested name already exists.");
            
            User owner = await _userRepository.FindSingleByExpressionAsync(user => user.Id.Equals(createTrainingRoomRequest.OwnerId));
            if (owner == null)
                return new CreateTrainingRoomResponse(createTrainingRoomRequest.Id, Guid.Empty, "User not found.");

            TrainingRoom trainingRoom = new TrainingRoom(owner, createTrainingRoomRequest.TrainingRoomName, createTrainingRoomRequest.TrainingRoomSettings);
            if (!await _trainingRoomRepository.CreateAsync(trainingRoom))
                return new CreateTrainingRoomResponse(createTrainingRoomRequest.Id, Guid.Empty, "Failed to create training room.");

            return new CreateTrainingRoomResponse(createTrainingRoomRequest.Id, trainingRoom.Id, "Successfully created a training room.", true);
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
        public Task<PostBrainScoresResponse> PostBrainScoresAsync(PostBrainScoresRequest postBrainScoresRequest)
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
        public async Task<GetEnabledTrainingRoomsResponse> GetEnabledTrainingRoomsAsync(GetEnabledTrainingRoomsRequest getEnabledTrainingRoomsRequest)
        {
            IEnumerable<TrainingRoom> trainingRooms = await _trainingRoomRepository.FindManyByExpressionAsync(trainingRoom => trainingRoom.Enabled);
            return new GetEnabledTrainingRoomsResponse(getEnabledTrainingRoomsRequest.Id, trainingRooms.Select(TrainingRoomToDto).ToList(), true);
        }

        private static TrainingRoomDto TrainingRoomToDto(TrainingRoom trainingRoom)
        {
            return new TrainingRoomDto()
            {
                Id = trainingRoom.Id,
                Name = trainingRoom.Name,
                Owner = UserToDto(trainingRoom.Owner),
                Generation = trainingRoom.Generation,
                HighestScore = trainingRoom.HighestScore,
                LowestScore = trainingRoom.LowestScore,
                AverageScore = trainingRoom.AverageScore,
                TrainingRoomSettings = trainingRoom.TrainingRoomSettings
            };
        }
        private static UserDto UserToDto(User user)
        {
            return new UserDto()
            {
                Id = user.Id,
                Name = user.Username,
                TimestampCreated = user.TimestampCreated
            };
        }
    }
}
