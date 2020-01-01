using AutoMapper;
using Neuralm.Services.TrainingRoomService.Domain;
using Neuralm.Services.TrainingRoomService.Messages.Dtos;

namespace Neuralm.Services.TrainingRoomService.Mapping.Profiles
{
    /// <summary>
    /// Represents the <see cref="ConnectionGeneProfile"/> class.
    /// </summary>
    public class ConnectionGeneProfile : Profile
    {
        /// <summary>
        /// Maps the profile.
        /// </summary>
        public ConnectionGeneProfile()
        {
            CreateMap<ConnectionGene, ConnectionGeneDto>();
            CreateMap<ConnectionGeneDto, ConnectionGene>();
        }
    }
}