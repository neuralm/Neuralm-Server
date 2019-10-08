using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Neuralm.Services.Common.Application.Abstractions;
using Neuralm.Services.Common.Application.Interfaces;
using Neuralm.Services.RegistryService.Application.Dtos;
using Neuralm.Services.RegistryService.Application.Interfaces;
using Neuralm.Services.RegistryService.Domain;

namespace Neuralm.Services.RegistryService.Application.Services
{
    /// <summary>
    /// Represents the <see cref="RegistryService"/> class.
    /// </summary>
    public class RegistryService : BaseService<Service, ServiceDto>, IRegistryService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryService"/> class.
        /// </summary>
        /// <param name="entityRepository">The entity repository.</param>
        /// <param name="mapper">The mapper.</patram>
        public RegistryService(IRepository<Service> entityRepository, IMapper mapper) : base(entityRepository, mapper)
        {

        }

        /// <inheritdoc />
        public async Task StartupAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public async Task StartMonitoringAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
