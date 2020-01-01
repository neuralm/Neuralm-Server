using AutoMapper;
using Neuralm.Services.TrainingRoomService.Domain;
using Neuralm.Services.TrainingRoomService.Messages.Dtos;

namespace Neuralm.Services.TrainingRoomService.Mapping.Profiles
{
    /// <summary>
    /// Represents the <see cref="OrganismProfile"/> class.
    /// </summary>
    public class OrganismProfile : Profile
    {
        /// <summary>
        /// Maps the profile.
        /// </summary>
        public OrganismProfile()
        {
            CreateMap<Organism, OrganismDto>();
            CreateMap<OrganismDto, Organism>();
        }
    }
}