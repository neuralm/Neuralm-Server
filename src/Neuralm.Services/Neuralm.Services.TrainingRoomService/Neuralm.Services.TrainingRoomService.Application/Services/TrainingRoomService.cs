using System;
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
            if (userDto is null || await EntityRepository.ExistsAsync(trainingRoom => trainingRoom.Name == dto.Name))
                return (false, Guid.Empty);
            return await base.CreateAsync(dto);
        }
    }
}
