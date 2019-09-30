using AutoMapper;
using Neuralm.Services.TrainingRoomService.Domain;
using Neuralm.Services.TrainingRoomService.Messages.Dtos;

namespace Neuralm.Services.TrainingRoomService.Mapping.Profiles
{
    /// <summary>
    /// Represents the <see cref="TrainingRoomProfile"/> class.
    /// </summary>
    public class TrainingRoomProfile : Profile
    {
        /// <summary>
        /// Maps the profile.
        /// </summary>
        public TrainingRoomProfile()
        {
            CreateMap<TrainingRoom, TrainingRoomDto>();
            CreateMap<TrainingRoomDto, TrainingRoom>();
        }
    }
}
