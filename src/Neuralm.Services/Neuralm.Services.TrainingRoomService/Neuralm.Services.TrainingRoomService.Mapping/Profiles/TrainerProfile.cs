using AutoMapper;
using Neuralm.Services.TrainingRoomService.Domain;
using Neuralm.Services.TrainingRoomService.Messages.Dtos;

namespace Neuralm.Services.TrainingRoomService.Mapping.Profiles
{
    /// <summary>
    /// Represents the <see cref="TrainerProfile"/> class.
    /// </summary>
    public class TrainerProfile : Profile
    {
        /// <summary>
        /// Maps the profile.
        /// </summary>
        public TrainerProfile()
        {
            CreateMap<Trainer, TrainerDto>();
            CreateMap<TrainerDto, Trainer>();
        }
    }
}