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
        #region DI Fields
        private readonly IRepository<TrainingRoom> _trainingRoomRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<TrainingSession> _trainingSessionRepository;
        #endregion DI Fields

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

            User owner = await _userRepository.FindSingleOrDefaultAsync(user => user.Id.Equals(createTrainingRoomRequest.OwnerId));
            TrainingRoom trainingRoom = new TrainingRoom(owner, createTrainingRoomRequest.TrainingRoomName, createTrainingRoomRequest.TrainingRoomSettings);
            if (!await _trainingRoomRepository.CreateAsync(trainingRoom))
                return new CreateTrainingRoomResponse(createTrainingRoomRequest.Id, Guid.Empty, "Failed to create training room.");

            return new CreateTrainingRoomResponse(createTrainingRoomRequest.Id, trainingRoom.Id, "Successfully created a training room.", true);
        }

        /// <inheritdoc cref="ITrainingRoomService.StartTrainingSessionAsync(StartTrainingSessionRequest)"/>
        public async Task<StartTrainingSessionResponse> StartTrainingSessionAsync(StartTrainingSessionRequest startTrainingSessionRequest)
        {
            TrainingRoom trainingRoom;
            Expression<Func<TrainingRoom, bool>> predicate = tr => tr.Id.Equals(startTrainingSessionRequest.TrainingRoomId);
            if ((trainingRoom = await _trainingRoomRepository.FindSingleOrDefaultAsync(predicate)) == default)
                return new StartTrainingSessionResponse(startTrainingSessionRequest.Id, null, "Training room does not exist.");
            if (!await _userRepository.ExistsAsync(usr => usr.Id.Equals(startTrainingSessionRequest.UserId)))
                return new StartTrainingSessionResponse(startTrainingSessionRequest.Id, null, "User does not exist.");

            if (!trainingRoom.IsUserAuthorized(startTrainingSessionRequest.UserId))
                return new StartTrainingSessionResponse(startTrainingSessionRequest.Id, null, "User is not authorized");
            if (!trainingRoom.StartTrainingSession(startTrainingSessionRequest.UserId, out TrainingSession trainingSession))
                return new StartTrainingSessionResponse(startTrainingSessionRequest.Id, null, "Failed to start a training session.");

            await _trainingRoomRepository.UpdateAsync(trainingRoom);

            TrainingSessionDto trainingSessionDto = EntityToDtoConverter.Convert<TrainingSessionDto, TrainingSession>(trainingSession);
            return new StartTrainingSessionResponse(startTrainingSessionRequest.Id, trainingSessionDto, "Successfully started a training session.", true);
        }

        /// <inheritdoc cref="ITrainingRoomService.EndTrainingSessionAsync(EndTrainingSessionRequest)"/>
        public async Task<EndTrainingSessionResponse> EndTrainingSessionAsync(EndTrainingSessionRequest endTrainingSessionRequest)
        {
            TrainingSession trainingSession;
            if ((trainingSession = await _trainingSessionRepository.FindSingleOrDefaultAsync(trs => trs.Id.Equals(endTrainingSessionRequest.TrainingSessionId))) == default)
                return new EndTrainingSessionResponse(endTrainingSessionRequest.Id, "Training session does not exist.");

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
            string message = "Successfully fetched all requested organisms.";
            TrainingSession trainingSession;
            if (getOrganismsRequest.TrainingSessionId.Equals(Guid.Empty))
                return new GetOrganismsResponse(getOrganismsRequest.Id, new List<OrganismDto>(), "Training room id cannot be an empty guid.");
            if (getOrganismsRequest.Amount < 1)
                return new GetOrganismsResponse(getOrganismsRequest.Id, new List<OrganismDto>(), "Amount cannot be smaller than 1.");
            if ((trainingSession = await _trainingSessionRepository.FindSingleOrDefaultAsync(ts => ts.Id.Equals(getOrganismsRequest.TrainingSessionId))) == default)
                return new GetOrganismsResponse(getOrganismsRequest.Id, new List<OrganismDto>(), "Training session does not exist.");

            // if the list is empty then get new ones from the training room
            if (trainingSession.LeasedOrganisms.Count(o => !o.Organism.Evaluated) == 0)
            {
                if (trainingSession.TrainingRoom.Generation == 0)
                {
                    TrainingRoomSettings trainingRoomSettings = trainingSession.TrainingRoom.TrainingRoomSettings;
                    for (int i = 0; i < trainingRoomSettings.OrganismCount; i++)
                    {
                        Organism organism = new Organism(trainingSession.TrainingRoom.Generation, trainingRoomSettings) { Leased = true };
                        trainingSession.TrainingRoom.AddOrganism(organism);
                        trainingSession.LeasedOrganisms.Add(new LeasedOrganism(organism));
                    }
                    trainingSession.TrainingRoom.IncreaseNodeIdTo(trainingRoomSettings.InputCount + trainingRoomSettings.OutputCount);
                    message = $"First generation; generated {trainingSession.TrainingRoom.TrainingRoomSettings.OrganismCount} organisms.";
                }
                else
                {
                    trainingSession.LeasedOrganisms.AddRange(GetNewLeasedOrganisms(getOrganismsRequest.Amount));
                    message = "Start of new generation.";
                    //// if the training room also is out of leasable organisms then perform EndGeneration
                    //// but only if all organisms are evaluated (maybe another training session is still evaluating the organisms that are being leased)
                    //if (!trainingSession.LeasedOrganisms.Any() && trainingSession.TrainingRoom.Species.SelectMany(p => p.Organisms).All(lo => lo.Evaluated))
                    //{
                    //    trainingSession.TrainingRoom.EndGeneration();
                    //    trainingSession.LeasedOrganisms.AddRange(GetNewLeasedOrganisms(getOrganismsRequest.Amount));
                    //    message = "Start of new generation.";
                    //}
                }
            }
            else if (trainingSession.LeasedOrganisms.Count(o => !o.Organism.Evaluated) < getOrganismsRequest.Amount)
            {
                int take = getOrganismsRequest.Amount - trainingSession.LeasedOrganisms.Count(o => !o.Organism.Evaluated);
                List<LeasedOrganism> newLeasedOrganisms = GetNewLeasedOrganisms(take);
                trainingSession.LeasedOrganisms.AddRange(newLeasedOrganisms);
            }
            
            if (trainingSession.LeasedOrganisms.Count(o => !o.Organism.Evaluated) < getOrganismsRequest.Amount)
                message = "The requested amount of organisms are not all available. The training room is probably close to a new generation or is waiting on other training sessions to complete.";

            await _trainingSessionRepository.UpdateAsync(trainingSession);

            List<OrganismDto> organismDtos = trainingSession.LeasedOrganisms
                .Where(lo => !lo.Organism.Evaluated)
                .Take(getOrganismsRequest.Amount)
                .Select(lo => EntityToDtoConverter.Convert<OrganismDto, Organism>(lo.Organism)).ToList();

            return new GetOrganismsResponse(getOrganismsRequest.Id, organismDtos, message, organismDtos.Any());

            List<LeasedOrganism> GetNewLeasedOrganisms(int take)
            {
                return trainingSession.TrainingRoom.Species.SelectMany(sp => sp.Organisms).Where(lo => !lo.Leased)
                    .Take(take).Select(o =>
                    {
                        o.Leased = true;
                        return new LeasedOrganism(o);
                    }).ToList();
            }
        }

        /// <inheritdoc cref="ITrainingRoomService.PostOrganismsScoreAsync(PostOrganismsScoreRequest)"/>
        public async Task<PostOrganismsScoreResponse> PostOrganismsScoreAsync(PostOrganismsScoreRequest postOrganismsScoreRequest)
        {
            TrainingSession trainingSession = await _trainingSessionRepository.FindSingleOrDefaultAsync(p => p.Id.Equals(postOrganismsScoreRequest.TrainingSessionId));
            if (trainingSession == default)
                return new PostOrganismsScoreResponse(postOrganismsScoreRequest.Id, "Training session does not exist.");
            int count = 0;

            List<LeasedOrganism> orgs = postOrganismsScoreRequest.OrganismScores
                .Select(o =>
                {
                    LeasedOrganism oo = trainingSession.LeasedOrganisms.SingleOrDefault(a => a.OrganismId.Equals(o.Key));
                    if (oo == default)
                        count++;
                    return oo;
                }).ToList();

            if (count > 0)
                return new PostOrganismsScoreResponse(postOrganismsScoreRequest.Id, $"{count} of the organisms does not exist in the training session.");

            foreach (LeasedOrganism leasedOrganism in orgs)
            {
                trainingSession.TrainingRoom.PostScore(leasedOrganism.Organism, postOrganismsScoreRequest.OrganismScores[leasedOrganism.OrganismId]);
            }

            string message = "Successfully updated the organisms scores.";
            if (trainingSession.TrainingRoom.Species.SelectMany(p => p.Organisms).All(lo => lo.Evaluated))
            {
                message = trainingSession.TrainingRoom.EndGeneration() 
                    ? "Successfully updated the organisms and advanced a generation!" 
                    : "Successfully updated the organisms but failed to advance a generation!";
                trainingSession.LeasedOrganisms.Clear();
            }
            await _trainingSessionRepository.UpdateAsync(trainingSession);
            return new PostOrganismsScoreResponse(postOrganismsScoreRequest.Id, message, true);
        }
    }
}
