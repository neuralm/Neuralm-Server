using System;
using System.Collections.Concurrent;
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
        #region DI Fields
        private readonly IRepository<TrainingRoom> _trainingRoomRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<TrainingSession> _trainingSessionRepository;
        #endregion DI Fields

        #region Fields
        private readonly ConcurrentDictionary<Guid, ConcurrentDictionary<Organism, bool>> _trainingSessionOrganismsDictionary;
        #endregion Fields

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
            _trainingSessionOrganismsDictionary = new ConcurrentDictionary<Guid, ConcurrentDictionary<Organism, bool>>();
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

            User owner = await _userRepository.FindSingleOrDefaultAsync(user => user.Id.Equals(createTrainingRoomRequest.OwnerId));
            TrainingRoom trainingRoom = new TrainingRoom(owner, createTrainingRoomRequest.TrainingRoomName, createTrainingRoomRequest.TrainingRoomSettings);
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

            TrainingRoom trainingRoom = await _trainingRoomRepository.FindSingleOrDefaultAsync(predicate);
            if (!trainingRoom.IsUserAuthorized(startTrainingSessionRequest.UserId))
                return new StartTrainingSessionResponse(startTrainingSessionRequest.Id, null, "User is not authorized");
            if (!trainingRoom.StartTrainingSession(startTrainingSessionRequest.UserId, out TrainingSession trainingSession))
                return new StartTrainingSessionResponse(startTrainingSessionRequest.Id, null, "Failed to start a training session.");

            await _trainingRoomRepository.UpdateAsync(trainingRoom);
            _trainingSessionOrganismsDictionary.TryAdd(trainingSession.Id, new ConcurrentDictionary<Organism, bool>(GetOrganismsFree(trainingRoom)));

            TrainingSessionDto trainingSessionDto = EntityToDtoConverter.Convert<TrainingSessionDto, TrainingSession>(trainingSession);
            return new StartTrainingSessionResponse(startTrainingSessionRequest.Id, trainingSessionDto, "Successfully started a training session.", true);
        }

        /// <inheritdoc cref="ITrainingRoomService.EndTrainingSessionAsync(EndTrainingSessionRequest)"/>
        public async Task<EndTrainingSessionResponse> EndTrainingSessionAsync(EndTrainingSessionRequest endTrainingSessionRequest)
        {
            Expression<Func<TrainingSession, bool>> predicate = trs => trs.Id.Equals(endTrainingSessionRequest.TrainingSessionId);
            if (!await _trainingSessionRepository.ExistsAsync(predicate))
                return new EndTrainingSessionResponse(endTrainingSessionRequest.Id, "Training session does not exist.");

            TrainingSession trainingSession = await _trainingSessionRepository.FindSingleOrDefaultAsync(predicate);
            if (trainingSession.EndedTimestamp != default)
                return new EndTrainingSessionResponse(endTrainingSessionRequest.Id, "Training session was already ended.");

            trainingSession.EndTrainingSession();
            await _trainingSessionRepository.UpdateAsync(trainingSession);
            return new EndTrainingSessionResponse(endTrainingSessionRequest.Id, "Successfully ended the training session.", true);
        }

        /// <inheritdoc cref="ITrainingRoomService.GetEnabledTrainingRoomsAsync(GetEnabledTrainingRoomsRequest)"/>
        public async Task<GetEnabledTrainingRoomsResponse> GetEnabledTrainingRoomsAsync(GetEnabledTrainingRoomsRequest getEnabledTrainingRoomsRequest)
        {
            IEnumerable<TrainingRoom> trainingRooms = await _trainingRoomRepository.FindManyAsync(trainingRoom => trainingRoom.Enabled);
            List<TrainingRoomDto> trainingRoomDtos = trainingRooms.Select(EntityToDtoConverter.Convert<TrainingRoomDto, TrainingRoom>).ToList();
            return new GetEnabledTrainingRoomsResponse(getEnabledTrainingRoomsRequest.Id, trainingRoomDtos, success: true);
        }

        /// <inheritdoc cref="ITrainingRoomService.GetOrganismsAsync(GetOrganismsRequest)"/>
        public async Task<GetOrganismsResponse> GetOrganismsAsync(GetOrganismsRequest getOrganismsRequest)
        {
            Expression<Func<TrainingSession, bool>> predicate = ts => ts.Id.Equals(getOrganismsRequest.TrainingSessionId);
            if (getOrganismsRequest.TrainingSessionId.Equals(Guid.Empty))
                return new GetOrganismsResponse(getOrganismsRequest.Id, new List<OrganismDto>(), "Training room id cannot be an empty guid.");
            if (getOrganismsRequest.Amount < 1)
                return new GetOrganismsResponse(getOrganismsRequest.Id, new List<OrganismDto>(), "Amount cannot be smaller than 1.");
            if (!await _trainingSessionRepository.ExistsAsync(predicate))
                return new GetOrganismsResponse(getOrganismsRequest.Id, new List<OrganismDto>(), "Training session does not exist.");

            if (!_trainingSessionOrganismsDictionary.TryGetValue(getOrganismsRequest.TrainingSessionId, out ConcurrentDictionary<Organism, bool> organisms) || organisms.IsEmpty)
            {
                TrainingSession trainingSession = await _trainingSessionRepository.FindSingleOrDefaultAsync(predicate);
                TrainingRoom trainingRoom = trainingSession.TrainingRoom;

                // If the training room is in its first generation, initialize the first species with the training room settings
                if (trainingRoom.Generation == 0)
                {
                    for (int i = 0; i < trainingRoom.TrainingRoomSettings.OrganismCount; i++)
                    {
                        Organism organism = new Organism(trainingRoom);
                        trainingRoom.AddOrganism(organism);
                        organisms.TryAdd(organism, true);
                    }
                    // NOTE: The organisms list (in training room) does not get updated because it is not in the last generation...
                    await _trainingSessionRepository.UpdateAsync(trainingSession);
                }
                else
                {
                    organisms = _trainingSessionOrganismsDictionary.AddOrUpdate(
                        trainingSession.Id,
                        new ConcurrentDictionary<Organism, bool>(GetOrganismsFree(trainingRoom)),
                        (guid, dictionary) =>
                        {
                            foreach ((Organism organism, bool value) in GetOrganismsFree(trainingRoom))
                            {
                                dictionary.TryAdd(organism, value);
                            }
                            return dictionary;
                        });
                }
            }

            List<OrganismDto> organismDtos = new List<OrganismDto>();
            for (int i = 0; i < getOrganismsRequest.Amount; i++)
            {
                (Organism organism, _) = organisms.FirstOrDefault(p => p.Value);
                if (organism != default)
                {
                    organisms[organism] = false;
                    organismDtos.Add(EntityToDtoConverter.Convert<OrganismDto, Organism>(organism));
                }
                else
                    break;
            }
            return organismDtos.Any() 
                ? new GetOrganismsResponse(getOrganismsRequest.Id, organismDtos, "Successfully fetched the organisms.", true) 
                : new GetOrganismsResponse(getOrganismsRequest.Id, organismDtos, "The organism bag is empty.", false);
        }

        /// <inheritdoc cref="ITrainingRoomService.PostOrganismsScoreAsync(PostOrganismsScoreRequest)"/>
        public async Task<PostOrganismsScoreResponse> PostOrganismsScoreAsync(PostOrganismsScoreRequest postOrganismsScoreRequest)
        {
            TrainingSession trainingSession = await _trainingSessionRepository.FindSingleOrDefaultAsync(p => p.Id.Equals(postOrganismsScoreRequest.TrainingSessionId));
            if (trainingSession == default)
                return new PostOrganismsScoreResponse(postOrganismsScoreRequest.Id, "Training session does not exist.");
            ConcurrentDictionary<Organism, bool> orgs = _trainingSessionOrganismsDictionary[postOrganismsScoreRequest.TrainingSessionId];
            foreach (Organism organism in postOrganismsScoreRequest.OrganismScores.Select(o => orgs.SingleOrDefault(a => a.Key.Id.Equals(o.Key)).Key))
            {
                if (organism == default)
                    return new PostOrganismsScoreResponse(postOrganismsScoreRequest.Id, "One of the organisms does not exist in the training room.");
                trainingSession.TrainingRoom.PostScore(organism, postOrganismsScoreRequest.OrganismScores[organism.Id]);
            }

            await _trainingSessionRepository.UpdateAsync(trainingSession);
            return new PostOrganismsScoreResponse(postOrganismsScoreRequest.Id, "Successfully updated the organisms scores.", true);
        }

        private static IEnumerable<KeyValuePair<Organism, bool>> GetOrganismsFree(TrainingRoom trainingRoom)
        {
            return trainingRoom.Species.SelectMany(sp => sp.LastGenerationOrganisms).Select(a => new KeyValuePair<Organism, bool>(a, true));
        }
    }
}
