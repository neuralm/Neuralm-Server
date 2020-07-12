using AutoMapper;
using Neuralm.Services.Common.Domain;
using Neuralm.Services.Common.Messages.Dtos;

namespace Neuralm.Services.Common.Mapping.Profiles
{
    /// <summary>
    /// Represents the <see cref="HealthCheckProfile"/> class.
    /// </summary>
    public class HealthCheckProfile : Profile
    {
        /// <summary>
        /// Maps the profile.
        /// </summary>
        public HealthCheckProfile()
        {
            CreateMap<HealthCheck, HealthCheckDto>();
            CreateMap<HealthCheckDto, HealthCheck>();
        }
    }
}
