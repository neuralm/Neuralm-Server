using AutoMapper;
using Neuralm.Services.TrainingRoomService.Domain;
using Neuralm.Services.TrainingRoomService.Messages.Dtos;

namespace Neuralm.Services.TrainingRoomService.Mapping.Profiles
{
    /// <summary>
    /// Represents the <see cref="TrainingSessionProfile"/> class.
    /// </summary>
    public class TrainingSessionProfile : Profile
    {
        /// <summary>
        /// Maps the profile.
        /// </summary>
        public TrainingSessionProfile()
        {
            CreateMap<TrainingSession, TrainingSessionDto>();
            CreateMap<TrainingSessionDto, TrainingSession>();
        }
    }
}
