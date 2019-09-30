using AutoMapper;
using Neuralm.Services.TrainingRoomService.Domain;
using Neuralm.Services.TrainingRoomService.Messages.Dtos;

namespace Neuralm.Services.TrainingRoomService.Mapping.Profiles
{
    /// <summary>
    /// Represents the <see cref="TrainingRoomSettingsProfile"/> class.
    /// </summary>
    public class TrainingRoomSettingsProfile : Profile
    {
        /// <summary>
        /// Maps the profile.
        /// </summary>
        public TrainingRoomSettingsProfile()
        {
            CreateMap<TrainingRoomSettings, TrainingRoomSettingsDto>();
            CreateMap<TrainingRoomSettingsDto, TrainingRoomSettings>();
        }
    }
}
