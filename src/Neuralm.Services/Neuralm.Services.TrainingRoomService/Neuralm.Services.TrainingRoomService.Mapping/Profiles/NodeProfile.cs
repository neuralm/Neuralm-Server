using AutoMapper;
using Neuralm.Services.TrainingRoomService.Domain;
using Neuralm.Services.TrainingRoomService.Messages.Dtos;

namespace Neuralm.Services.TrainingRoomService.Mapping.Profiles
{
    /// <summary>
    /// Represents the <see cref="NodeProfile"/> class.
    /// </summary>
    public class NodeProfile : Profile
    {
        /// <summary>
        /// Maps the profile.
        /// </summary>
        public NodeProfile()
        {
            CreateMap<Node, NodeDto>();
            CreateMap<NodeDto, Node>();
        }
    }
}