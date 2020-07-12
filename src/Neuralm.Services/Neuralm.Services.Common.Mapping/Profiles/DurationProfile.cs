using AutoMapper;
using Neuralm.Services.Common.Domain;
using Neuralm.Services.Common.Messages.Dtos;

namespace Neuralm.Services.Common.Mapping.Profiles
{
    /// <summary>
    /// Represents the <see cref="DurationProfile"/> class.
    /// </summary>
    public class DurationProfile : Profile
    {
        /// <summary>
        /// Maps the profile.
        /// </summary>
        public DurationProfile()
        {
            CreateMap<Duration, DurationDto>();
            CreateMap<DurationDto, Duration>();
        }
    }
}
