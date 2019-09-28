using AutoMapper;
using Neuralm.Services.RegistryService.Application.Dtos;
using Neuralm.Services.RegistryService.Domain;

namespace Neuralm.Services.RegistryService.Mapping.Profiles
{
    /// <summary>
    /// Represents the <see cref="ServiceProfile"/> class.
    /// </summary>
    public class ServiceProfile : Profile
    {
        /// <summary>
        /// Maps the profile.
        /// </summary>
        public ServiceProfile()
        {
            CreateMap<Service, ServiceDto>();
            CreateMap<ServiceDto, Service>();
        }
    }
}
