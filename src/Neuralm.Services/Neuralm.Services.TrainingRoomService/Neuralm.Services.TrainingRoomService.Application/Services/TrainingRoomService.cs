using AutoMapper;
using Neuralm.Services.Common.Application.Abstractions;
using Neuralm.Services.Common.Application.Interfaces;
using Neuralm.Services.TrainingRoomService.Application.Interfaces;
using Neuralm.Services.TrainingRoomService.Domain;
using Neuralm.Services.TrainingRoomService.Messages.Dtos;

namespace Neuralm.Services.TrainingRoomService.Application.Services
{
    /// <summary>
    /// Represents the implementation of the <see cref="ITrainingRoomService"/> interface.
    /// </summary>
    public class TrainingRoomService : BaseService<TrainingRoom, TrainingRoomDto>, ITrainingRoomService
    {
        private readonly IRepository<TrainingSession> _trainingSessionRepository;

        /// <summary>
        /// Initializes an instance of the <see cref="TrainingRoomService"/> class.
        /// </summary>
        /// <param name="trainingRoomRepository">The training room repository.</param>
        /// <param name="trainingSessionRepository">The training session repository.</param>
        /// <param name="mapper">The mapper.</param>
        public TrainingRoomService(
            IRepository<TrainingRoom> trainingRoomRepository,
            IRepository<TrainingSession> trainingSessionRepository,
            IMapper mapper) : base(trainingRoomRepository, mapper)
        {
            _trainingSessionRepository = trainingSessionRepository;
        }

        /*
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

        /// <inheritdoc cref="ITrainingRoomService.GetEnabledTrainingRoomsAsync(GetEnabledTrainingRoomsRequest)"/>
        public async Task<GetEnabledTrainingRoomsResponse> GetEnabledTrainingRoomsAsync(GetEnabledTrainingRoomsRequest getEnabledTrainingRoomsRequest)
        {
            IEnumerable<TrainingRoom> trainingRooms = await _trainingRoomRepository.FindManyAsync(trainingRoom => trainingRoom.Enabled);
            List<TrainingRoomDto> trainingRoomDtos = trainingRooms.Select(EntityToDtoConverter.Convert<TrainingRoomDto, TrainingRoom>).ToList();
            return new GetEnabledTrainingRoomsResponse(getEnabledTrainingRoomsRequest.Id, trainingRoomDtos, success: true);
        }
        */
    }
}
