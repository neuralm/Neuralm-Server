using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Neuralm.Services.Common.Application.Abstractions;
using Neuralm.Services.Common.Application.Interfaces;
using Neuralm.Services.TrainingRoomService.Application.Interfaces;
using Neuralm.Services.TrainingRoomService.Domain;
using Neuralm.Services.TrainingRoomService.Messages;
using Neuralm.Services.TrainingRoomService.Messages.Dtos;

namespace Neuralm.Services.TrainingRoomService.Application.Services
{
    /// <summary>
    /// Represents the <see cref="TrainingSessionService"/> class.
    /// </summary>
    public class TrainingSessionService : BaseService<TrainingSession, TrainingSessionDto>, ITrainingSessionService
    {
        private readonly IRepository<TrainingRoom> _trainingRoomRepository;
        private readonly IUserService _userService;
        private readonly ILogger<TrainingSessionService> _logger;
        private readonly ITrainingSessionRepository _trainingSessionRepository;

        /// <summary>
        /// Initializes an instance of the <see cref="TrainingSessionService"/> class.
        /// </summary>
        /// <param name="trainingSessionRepository">The training session repository.</param>
        /// <param name="trainingRoomRepository">The training room repository.</param>
        /// <param name="userService">The user service.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logger">The logger.</param>
        public TrainingSessionService(
            ITrainingSessionRepository trainingSessionRepository,
            IRepository<TrainingRoom> trainingRoomRepository,
            IUserService userService,
            IMapper mapper,
            ILogger<TrainingSessionService> logger) : base(trainingSessionRepository, mapper)
        {
            _trainingSessionRepository = trainingSessionRepository;
            _trainingRoomRepository = trainingRoomRepository;
            _userService = userService;
            _logger = logger;
        }
        
        /// <inheritdoc cref="ITrainingSessionService.StartTrainingSessionAsync(StartTrainingSessionRequest)"/>
        public async Task<StartTrainingSessionResponse> StartTrainingSessionAsync(StartTrainingSessionRequest startTrainingSessionRequest)
        {
            TrainingRoom trainingRoom;
            if ((trainingRoom = await _trainingRoomRepository.FindSingleOrDefaultAsync(tr => tr.Id.Equals(startTrainingSessionRequest.TrainingRoomId))) == default)
                return new StartTrainingSessionResponse(startTrainingSessionRequest.Id, null, "Training room does not exist.");
            UserDto user = await _userService.FindUserAsync(startTrainingSessionRequest.UserId);
            if (user is null)
                return new StartTrainingSessionResponse(startTrainingSessionRequest.Id, null, "User does not exist.");
            if (!trainingRoom.IsUserAuthorized(startTrainingSessionRequest.UserId))
                return new StartTrainingSessionResponse(startTrainingSessionRequest.Id, null, "User is not authorized");
            if (!trainingRoom.StartTrainingSession(startTrainingSessionRequest.UserId, out TrainingSession trainingSession))
                return new StartTrainingSessionResponse(startTrainingSessionRequest.Id, null, "Training session already started.");
            await EntityRepository.CreateAsync(trainingSession);
            TrainingSessionDto trainingSessionDto = Mapper.Map<TrainingSessionDto>(trainingSession);
            return new StartTrainingSessionResponse(startTrainingSessionRequest.Id, trainingSessionDto, "Successfully started a training session.", true);
        }
        
        /// <inheritdoc cref="ITrainingSessionService.EndTrainingSessionAsync(EndTrainingSessionRequest)"/>
        public async Task<EndTrainingSessionResponse> EndTrainingSessionAsync(EndTrainingSessionRequest endTrainingSessionRequest)
        {
            TrainingSession trainingSession;
            if ((trainingSession = await EntityRepository.FindSingleOrDefaultAsync(trs => trs.Id.Equals(endTrainingSessionRequest.TrainingSessionId))) == default)
                return new EndTrainingSessionResponse(endTrainingSessionRequest.Id, "Training session does not exist.");
            if (trainingSession.EndedTimestamp != default)
                return new EndTrainingSessionResponse(endTrainingSessionRequest.Id, "Training session was already ended.");
            trainingSession.EndTrainingSession();
            await EntityRepository.UpdateAsync(trainingSession);
            return new EndTrainingSessionResponse(endTrainingSessionRequest.Id, "Successfully ended the training session.", true);
        }
        
        /// <inheritdoc cref="ITrainingSessionService.GetOrganismsAsync(GetOrganismsRequest)"/>
        public async Task<GetOrganismsResponse> GetOrganismsAsync(GetOrganismsRequest getOrganismsRequest)
        {
            string message = "Successfully fetched all requested organisms.";
            TrainingSession trainingSession;
            if (getOrganismsRequest.Amount < 1)
                return new GetOrganismsResponse(getOrganismsRequest.Id, new List<OrganismDto>(), "Amount cannot be smaller than 1.");
            if ((trainingSession = await EntityRepository.FindSingleOrDefaultAsync(ts => ts.Id.Equals(getOrganismsRequest.TrainingSessionId))) == default)
                return new GetOrganismsResponse(getOrganismsRequest.Id, new List<OrganismDto>(), "Training session does not exist.");
            if (!trainingSession.TrainingRoom.IsUserAuthorized(getOrganismsRequest.UserId) || trainingSession.UserId != getOrganismsRequest.UserId)
                return new GetOrganismsResponse(getOrganismsRequest.Id, new List<OrganismDto>(), "User is not authorized.");
            if (trainingSession.EndedTimestamp != default)
                return new GetOrganismsResponse(getOrganismsRequest.Id, new List<OrganismDto>(), "Training session has ended and can not be used any more.");
            TrainingRoomSettings trainingRoomSettings = trainingSession.TrainingRoom.TrainingRoomSettings;
            // if the list is empty then get new ones from the training room
            if (trainingSession.LeasedOrganisms.Count(o => !o.Organism.IsEvaluated) == 0)
            {
                if (trainingSession.TrainingRoom.Generation == 0)
                {
                    for (int i = 0; i < trainingRoomSettings.OrganismCount; i++)
                    {
                        Organism organism = new Organism(trainingRoomSettings, trainingSession.TrainingRoom.GetInnovationNumber) { IsLeased = true };
                        trainingSession.TrainingRoom.AddOrganism(organism);
                        trainingSession.LeasedOrganisms.Add(new LeasedOrganism(organism, trainingSession.Id));
                    }
                    trainingSession.TrainingRoom.IncreaseNodeIdTo(trainingRoomSettings.InputCount + trainingRoomSettings.OutputCount);
                    message = $"First generation; generated {trainingSession.TrainingRoom.TrainingRoomSettings.OrganismCount.ToString()} organisms.";
                }
                else
                {
                    trainingSession.LeasedOrganisms.AddRange(GetNewLeasedOrganisms(getOrganismsRequest.Amount));
                    message = "Start of new generation.";
                }
            }
            else if (trainingSession.LeasedOrganisms.Count(o => !o.Organism.IsEvaluated) < getOrganismsRequest.Amount)
            {
                int take = getOrganismsRequest.Amount - trainingSession.LeasedOrganisms.Count(o => !o.Organism.IsEvaluated);
                List<LeasedOrganism> newLeasedOrganisms = GetNewLeasedOrganisms(take);
                trainingSession.LeasedOrganisms.AddRange(newLeasedOrganisms);
            }

            if (trainingSession.LeasedOrganisms.Count(o => !o.Organism.IsEvaluated) < getOrganismsRequest.Amount && getOrganismsRequest.Amount < trainingRoomSettings.OrganismCount)
                message = "The requested amount of organisms are not all available. The training room is close to a new generation.";

            if (message.StartsWith("First generation;"))
            {
                await _trainingSessionRepository.InsertFirstGenerationAsync(trainingSession);
            }
            else
            {
                await EntityRepository.SaveChangesAsync();
            }

            List<OrganismDto> organismDtos = trainingSession.LeasedOrganisms
                .Where(lo => !lo.Organism.IsEvaluated)
                .Take(getOrganismsRequest.Amount)
                .Select(lo =>
                {
                    OrganismDto organismDto = Mapper.Map<OrganismDto>(lo.Organism);
                    // Because the input and output nodes are set using a Many To Many relation the nodes are converted separately.
                    organismDto.InputNodes = lo.Organism.Inputs.Select(input => Mapper.Map<NodeDto>(input.InputNode)).ToList();
                    organismDto.OutputNodes = lo.Organism.Outputs.Select(input => Mapper.Map<NodeDto>(input.OutputNode)).ToList();
                    return organismDto;
                }).ToList();

            return new GetOrganismsResponse(getOrganismsRequest.Id, organismDtos, message, organismDtos.Any());

            List<LeasedOrganism> GetNewLeasedOrganisms(int take)
            {
                return trainingSession.TrainingRoom.Species
                    .SelectMany(sp => sp.Organisms)
                    .Where(lo => !lo.IsLeased)
                    .Take(take).Select(o =>
                    {
                        o.IsLeased = true;
                        return new LeasedOrganism(o, trainingSession.Id);
                    }).ToList();
            }
        }

        /// <inheritdoc cref="ITrainingSessionService.PostOrganismsScoreAsync(PostOrganismsScoreRequest)"/>
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
                leasedOrganism.Organism.Score = postOrganismsScoreRequest.OrganismScores.Find(o => o.Key == leasedOrganism.OrganismId).Value;
                leasedOrganism.Organism.IsEvaluated = true;
            }

            await _trainingSessionRepository.SaveChangesAsync();
            _logger.LogInformation("UPDATED ORGANISM SCORES!!!");

            string message = "Successfully updated the organisms scores.";
            if (trainingSession.TrainingRoom.Species.SelectMany(p => p.Organisms).All(lo => lo.IsEvaluated))
            {
                trainingSession.LeasedOrganisms.Clear();
                await _trainingSessionRepository.SaveChangesAsync();
                _logger.LogInformation("CLEARED LEASED ORGANISMS AND SAVED CHANGES!!!");
                
                if (trainingSession.TrainingRoom.AllOrganismsInCurrentGenerationAreEvaluated())
                {
                    trainingSession.TrainingRoom.EndGeneration((organism) => { _trainingSessionRepository.MarkAsAdded(organism); });
                    message = "Successfully updated the organisms and advanced a generation!";
                }
                else
                {
                    message = "Successfully updated the organisms but failed to advance a generation!";
                }

                await _trainingSessionRepository.UpdateOrganismsAsync(trainingSession);
                _logger.LogInformation("ENDED THE GENERATION!!!");
            }
            return new PostOrganismsScoreResponse(postOrganismsScoreRequest.Id, message, true);
        }
    }
}
