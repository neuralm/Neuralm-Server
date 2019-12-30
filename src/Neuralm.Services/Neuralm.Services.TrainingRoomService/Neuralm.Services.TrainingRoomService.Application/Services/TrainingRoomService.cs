using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly IUserService _userService;

        /// <summary>
        /// Initializes an instance of the <see cref="TrainingRoomService"/> class.
        /// </summary>
        /// <param name="trainingRoomRepository">The training room repository.</param>
        /// <param name="trainingSessionRepository">The training session repository.</param>
        /// <param name="userService">The user service.</param>
        /// <param name="mapper">The mapper.</param>
        public TrainingRoomService(
            IRepository<TrainingRoom> trainingRoomRepository,
            IRepository<TrainingSession> trainingSessionRepository,
            IUserService userService,
            IMapper mapper) : base(trainingRoomRepository, mapper)
        {
            _trainingSessionRepository = trainingSessionRepository;
            _userService = userService;
        }

        /// <inheritdoc cref="ITrainingRoomService.CreateAsync"/>
        public override async Task<(bool success, Guid id)> CreateAsync(TrainingRoomDto dto)
        {
            /**
             * NOTE: It is now unclear how the request could have failed...
             * Previous validation messages:
             * "The ownerId must not be an empty guid."
             * "The training room name cannot be null or be empty"
             * "A training room with the requested name already exists."
             * "User not found."
             */
            UserDto userDto = await _userService.FindUserAsync(dto.Owner.Id);
            if (userDto is null ||
                await EntityRepository.ExistsAsync(trainingRoom => trainingRoom.Name == dto.Name))
                return (false, Guid.Empty);
            dto.Id = Guid.NewGuid();
            dto.OwnerId = userDto.Id;
            dto.AuthorizedTrainers = new List<TrainerDto>
            {
                new TrainerDto() {UserId = dto.OwnerId, TrainingRoomId = dto.Id}
            };
            dto.TrainingSessions = new List<TrainingSessionDto>();
            return await base.CreateAsync(dto);
        }

        /// <inheritdoc cref="ITrainingRoomService.GetAllAsync"/>
        public override async Task<IEnumerable<TrainingRoomDto>> GetAllAsync()
        {
            IEnumerable<TrainingRoomDto> trainingRooms = await base.GetAllAsync();
            return EnsureOwner(trainingRooms);
        }
        
        /// <inheritdoc cref="ITrainingRoomService.GetPaginationAsync(int, int)"/>
        public override async Task<IEnumerable<TrainingRoomDto>> GetPaginationAsync(int pageNumber, int pageSize)
        {
            IEnumerable<TrainingRoomDto> trainingRooms = await base.GetPaginationAsync(pageNumber, pageSize);
            return EnsureOwner(trainingRooms);
        }

        /// <inheritdoc cref="ITrainingRoomService.FindSingleOrDefaultAsync(Guid)"/>
        public override async Task<TrainingRoomDto> FindSingleOrDefaultAsync(Guid id)
        {
            TrainingRoomDto trainingRoomDto = await base.FindSingleOrDefaultAsync(id);
            if (trainingRoomDto is null)
                return null;
            return await EnsureOwner(trainingRoomDto);
        }

        /// <summary>
        /// Ensures that the owner property is set.
        /// This property can be unset when not in cache.
        /// </summary>
        /// <param name="trainingRooms">The training rooms.</param>
        /// <returns>Returns the fixed training rooms.</returns>
        private IEnumerable<TrainingRoomDto> EnsureOwner(IEnumerable<TrainingRoomDto> trainingRooms)
        {
            return trainingRooms.Select(async dto => await EnsureOwner(dto)).Select(task => task.Result);
        }

        /// <summary>
        /// Ensures that the owner property is set.
        /// This property can be unset when not in cache.
        /// </summary>
        /// <param name="trainingRoomDto">The training room.</param>
        /// <returns>Returns the fixed training room.</returns>
        private async Task<TrainingRoomDto> EnsureOwner(TrainingRoomDto trainingRoomDto)
        {
            if (!(trainingRoomDto.Owner is null)) return trainingRoomDto;
            UserDto userDto = await _userService.FindUserAsync(trainingRoomDto.OwnerId);
            trainingRoomDto.Owner = userDto;
            trainingRoomDto.AuthorizedTrainers = trainingRoomDto.AuthorizedTrainers.Select(EnsureTrainer).Select(task => task.Result).ToList();
            return trainingRoomDto;
        }

        /// <summary>
        /// Ensures that the user property is set.
        /// This property can be unset when not in cache.
        /// </summary>
        /// <param name="trainerDto">The trainer.</param>
        /// <returns>Returns the fixed trainer.</returns>
        private async Task<TrainerDto> EnsureTrainer(TrainerDto trainerDto)
        {
            if (!(trainerDto.User is null)) return trainerDto;
            UserDto userDto = await _userService.FindUserAsync(trainerDto.UserId);
            trainerDto.User = userDto;
            return trainerDto;
        }
    }
}
