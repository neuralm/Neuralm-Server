using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        private readonly IRepository<TrainingSession> _trainingSessionRepository;

        public TrainingRoomService(
            IRepository<TrainingRoom> trainingRoomRepository,
            IRepository<User> userRepository,
            IRepository<TrainingSession> trainingSessionRepository)
        {
            _trainingRoomRepository = trainingRoomRepository;
            _userRepository = userRepository;
            _trainingSessionRepository = trainingSessionRepository;
        }

        public async Task<CreateTrainingRoomResponse> CreateTrainingRoomAsync(CreateTrainingRoomRequest createTrainingRoomRequest)
        {
            if (createTrainingRoomRequest.OwnerId.Equals(Guid.Empty))
                return new CreateTrainingRoomResponse(createTrainingRoomRequest.Id, Guid.Empty, "The ownerId must not be an empty guid.");

            if (string.IsNullOrWhiteSpace(createTrainingRoomRequest.TrainingRoomName))
                return new CreateTrainingRoomResponse(createTrainingRoomRequest.Id, Guid.Empty, "The training room name cannot be null or be empty");

            if (await _trainingRoomRepository.ExistsAsync(tr => tr.Name.Equals(createTrainingRoomRequest.TrainingRoomName, StringComparison.CurrentCultureIgnoreCase)))
                return new CreateTrainingRoomResponse(createTrainingRoomRequest.Id, Guid.Empty, "A training room with the requested name already exists.");

            if (!await _userRepository.ExistsAsync(user => user.Id.Equals(createTrainingRoomRequest.OwnerId)))
                return new CreateTrainingRoomResponse(createTrainingRoomRequest.Id, Guid.Empty, "User not found.");

            User owner = await _userRepository.FindSingleByExpressionAsync(user => user.Id.Equals(createTrainingRoomRequest.OwnerId));
            TrainingRoom trainingRoom = new TrainingRoom(owner, createTrainingRoomRequest.TrainingRoomName, createTrainingRoomRequest.TrainingRoomSettings);
            if (!await _trainingRoomRepository.CreateAsync(trainingRoom))
                return new CreateTrainingRoomResponse(createTrainingRoomRequest.Id, Guid.Empty, "Failed to create training room.");

            return new CreateTrainingRoomResponse(createTrainingRoomRequest.Id, trainingRoom.Id, "Successfully created a training room.", true);
        }
        public async Task<StartTrainingSessionResponse> StartTrainingSessionAsync(StartTrainingSessionRequest startTrainingSessionRequest)
        {
            Expression<Func<TrainingRoom, bool>> predicate = tr => tr.Id.Equals(startTrainingSessionRequest.TrainingRoomId);
            if (!await _trainingRoomRepository.ExistsAsync(predicate))
                return new StartTrainingSessionResponse(startTrainingSessionRequest.Id, null, "Training room does not exist.");
            if (!await _userRepository.ExistsAsync(usr => usr.Id.Equals(startTrainingSessionRequest.UserId)))
                return new StartTrainingSessionResponse(startTrainingSessionRequest.Id, null, "User does not exist.");

            TrainingRoom trainingRoom = await _trainingRoomRepository.FindSingleByExpressionAsync(predicate);
            if (!trainingRoom.IsUserAuthorized(startTrainingSessionRequest.UserId))
                return new StartTrainingSessionResponse(startTrainingSessionRequest.Id, null, "User is not authorized");
            if (!trainingRoom.StartTrainingSession(startTrainingSessionRequest.UserId, out TrainingSession trainingSession))
                return new StartTrainingSessionResponse(startTrainingSessionRequest.Id, null, "Failed to start a training session.");

            await _trainingRoomRepository.UpdateAsync(trainingRoom);
            return new StartTrainingSessionResponse(startTrainingSessionRequest.Id, TrainingSessionToDto(trainingSession), "Successfully started a training session.", true);
        }
        public async Task<EndTrainingSessionResponse> EndTrainingSessionAsync(EndTrainingSessionRequest endTrainingSessionRequest)
        {
            Expression<Func<TrainingSession, bool>> predicate = trs => trs.Id.Equals(endTrainingSessionRequest.TrainingSessionId);
            if (!await _trainingSessionRepository.ExistsAsync(predicate))
                return new EndTrainingSessionResponse(endTrainingSessionRequest.Id, "Training session does not exist.");

            TrainingSession trainingSession = await _trainingSessionRepository.FindSingleByExpressionAsync(predicate);
            if (trainingSession.EndedTimestamp != default)
                return new EndTrainingSessionResponse(endTrainingSessionRequest.Id, "Training session was already ended.");

            trainingSession.EndTrainingSession();
            await _trainingSessionRepository.UpdateAsync(trainingSession);
            return new EndTrainingSessionResponse(endTrainingSessionRequest.Id, "Successfully ended the training session.", true);
        }
        public async Task<GetEnabledTrainingRoomsResponse> GetEnabledTrainingRoomsAsync(GetEnabledTrainingRoomsRequest getEnabledTrainingRoomsRequest)
        {
            IEnumerable<TrainingRoom> trainingRooms = await _trainingRoomRepository.FindManyByExpressionAsync(trainingRoom => trainingRoom.Enabled);
            return new GetEnabledTrainingRoomsResponse(getEnabledTrainingRoomsRequest.Id, trainingRooms.Select(TrainingRoomToDto).ToList(), true);
        }

        private static TrainingSessionDto TrainingSessionToDto(TrainingSession trainingSession)
        {
            return new TrainingSessionDto
            {
                Id = trainingSession.Id,
                StartedTimestamp = trainingSession.StartedTimestamp,
                EndedTimestamp = trainingSession.EndedTimestamp,
                TrainingRoom = TrainingRoomToDto(trainingSession.TrainingRoom),
                UserId = trainingSession.UserId
            };
        }
        private static TrainingRoomDto TrainingRoomToDto(TrainingRoom trainingRoom)
        {
            return new TrainingRoomDto
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
