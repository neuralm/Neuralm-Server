using AutoMapper;
using Neuralm.Services.TrainingRoomService.Domain;
using Neuralm.Services.TrainingRoomService.Messages.Dtos;

namespace Neuralm.Services.TrainingRoomService.Mapping.Profiles
{
    /// <summary>
    /// Represents the <see cref="SpeciesProfile"/> class.
    /// </summary>
    public class SpeciesProfile : Profile
    {
        /// <summary>
        /// Maps the profile.
        /// </summary>
        public SpeciesProfile()
        {
            CreateMap<Species, SpeciesDto>();
            CreateMap<SpeciesDto, Species>();
        }
    }
}