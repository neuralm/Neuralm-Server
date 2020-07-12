using AutoMapper;
using Neuralm.Services.Common.Domain;
using Neuralm.Services.Common.Messages.Dtos;

namespace Neuralm.Services.Common.Mapping.Profiles
{
    /// <summary>
    /// Represents the <see cref="ServiceHealthReportProfile"/> class.
    /// </summary>
    public class ServiceHealthReportProfile : Profile
    {
        /// <summary>
        /// Maps the profile.
        /// </summary>
        public ServiceHealthReportProfile()
        {
            CreateMap<ServiceHealthReport, ServiceHealthReportDto>();
            CreateMap<ServiceHealthReportDto, ServiceHealthReport>();
        }
    }
}
