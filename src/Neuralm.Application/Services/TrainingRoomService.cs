using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Neuralm.Application.Converters;
using Neuralm.Application.Interfaces;
using Neuralm.Application.Messages.Dtos;
using Neuralm.Application.Messages.Requests;
using Neuralm.Application.Messages.Responses;
using Neuralm.Domain.Entities;
using Neuralm.Domain.Entities.NEAT;

namespace Neuralm.Application.Services
{
    /// <summary>
    /// Represents the implementation of the <see cref="ITrainingRoomService"/> interface.
    /// </summary>
    public class TrainingRoomService : ITrainingRoomService
    {
        private readonly IRepository<TrainingRoom> _trainingRoomRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<TrainingSession> _trainingSessionRepository;

        /// <summary>
        /// Initializes an instance of the <see cref="TrainingRoomService"/> class.
        /// </summary>
        /// <param name="trainingRoomRepository">The training room repository.</param>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="trainingSessionRepository">The training session repository.</param>
        public TrainingRoomService(
            IRepository<TrainingRoom> trainingRoomRepository,
            IRepository<User> userRepository,
            IRepository<TrainingSession> trainingSessionRepository)
        {
            _trainingRoomRepository = trainingRoomRepository;
            _userRepository = userRepository;
            _trainingSessionRepository = trainingSessionRepository;
        }

        /// <inheritdoc cref="ITrainingRoomService.CreateTrainingRoomAsync(CreateTrainingRoomRequest)"/>
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
            Random random = new Random(trainingRoom.TrainingRoomSettings.Seed);
            for (int i = 0; i < 300; i++)
            {
                foreach (Species species in trainingRoom.Species)
                {
                    foreach (Organism organism in species.Organisms)
                    {
                        trainingRoom.PostScore(organism, random.NextDouble() + 0.001);
                    }
                }
                trainingRoom.EndGeneration();
                Console.WriteLine("generation " + i);
            }
            if (!await _trainingRoomRepository.CreateAsync(trainingRoom))
                return new CreateTrainingRoomResponse(createTrainingRoomRequest.Id, Guid.Empty, "Failed to create training room.");

            return new CreateTrainingRoomResponse(createTrainingRoomRequest.Id, trainingRoom.Id, "Successfully created a training room.", true);
        }

        /// <inheritdoc cref="ITrainingRoomService.StartTrainingSessionAsync(StartTrainingSessionRequest)"/>
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
            return new StartTrainingSessionResponse(startTrainingSessionRequest.Id, EntityToDtoConverter.Convert<TrainingSessionDto, TrainingSession>(trainingSession), "Successfully started a training session.", true);
        }

        /// <inheritdoc cref="ITrainingRoomService.EndTrainingSessionAsync(EndTrainingSessionRequest)"/>
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

        /// <inheritdoc cref="ITrainingRoomService.GetEnabledTrainingRoomsAsync(GetEnabledTrainingRoomsRequest)"/>
        public async Task<GetEnabledTrainingRoomsResponse> GetEnabledTrainingRoomsAsync(GetEnabledTrainingRoomsRequest getEnabledTrainingRoomsRequest)
        {
            IEnumerable<TrainingRoom> trainingRooms = await _trainingRoomRepository.FindManyByExpressionAsync(trainingRoom => trainingRoom.Enabled);
            return new GetEnabledTrainingRoomsResponse(getEnabledTrainingRoomsRequest.Id, trainingRooms.Select(EntityToDtoConverter.Convert<TrainingRoomDto, TrainingRoom>).ToList(), success: true);
        }

        /// <inheritdoc cref="ITrainingRoomService.GetOrganismsAsync(GetOrganismsRequest)"/>
        public async Task<GetOrganismsResponse> GetOrganismsAsync(GetOrganismsRequest getOrganismsRequest)
        {
            Expression<Func<TrainingSession, bool>> predicate = ts => ts.Id.Equals(getOrganismsRequest.TrainingSessionId);
            if (getOrganismsRequest.TrainingSessionId.Equals(Guid.Empty))
                return new GetOrganismsResponse(getOrganismsRequest.Id, null, "Training room id cannot be an empty guid.");

            if (!await _trainingSessionRepository.ExistsAsync(predicate))
                return new GetOrganismsResponse(getOrganismsRequest.Id, null, "Training session does not exist.");

            TrainingSession trainingSession = await _trainingSessionRepository.FindSingleByExpressionAsync(predicate);
            TrainingRoom trainingRoom = trainingSession.TrainingRoom;
            List<OrganismDto> organisms = trainingRoom.Species.SelectMany(sp =>
                    sp.LastGenerationOrganisms.Select(EntityToDtoConverter.Convert<OrganismDto, Organism>).ToList())
                .ToList();
            return new GetOrganismsResponse(getOrganismsRequest.Id, organisms, "Successfully fetched the organisms.", true);
        }

        /// <inheritdoc cref="ITrainingRoomService.PostOrganismsScoreAsync(PostOrganismsScoreRequest)"/>
        public async Task<PostOrganismsScoreResponse> PostOrganismsScoreAsync(PostOrganismsScoreRequest postOrganismsScoreRequest)
        {
            throw new NotImplementedException();
        }
    }
}
