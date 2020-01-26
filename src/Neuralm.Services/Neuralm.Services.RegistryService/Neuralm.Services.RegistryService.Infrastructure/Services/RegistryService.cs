using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Neuralm.Services.Common.Application.Abstractions;
using Neuralm.Services.Common.Application.Interfaces;
using Neuralm.Services.Common.Infrastructure.Networking;
using Neuralm.Services.RegistryService.Application.Configurations;
using Neuralm.Services.RegistryService.Application.Dtos;
using Neuralm.Services.RegistryService.Domain;
using Neuralm.Services.RegistryService.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IRegistryService = Neuralm.Services.RegistryService.Application.Interfaces.IRegistryService;

namespace Neuralm.Services.RegistryService.Infrastructure.Services
{
    /// <summary>
    /// Represents the <see cref="RegistryService"/> class.
    /// </summary>
    public class RegistryService : BaseService<Service, ServiceDto>, IRegistryService
    {
        private readonly IRepository<Service> _serviceRepository;
        private readonly INetworkConnector _networkConnector;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryService"/> class.
        /// </summary>
        /// <param name="serviceRepository">The service repository.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="neuralmConfigurationOptions">The neuralm configuration options.</param>
        /// <param name="messageTypeCache">The message type cache.</param>
        /// <param name="messageSerializer">The message serializer.</param>
        /// <param name="messageProcessor">The message processor.</param>
        /// <param name="tcpNetworkConnectorLogger">The tcp network connector logger.</param>
        public RegistryService(
            IRepository<Service> serviceRepository, 
            IMapper mapper,
            IOptions<NeuralmConfiguration> neuralmConfigurationOptions,
            IMessageTypeCache messageTypeCache,
            IMessageSerializer messageSerializer,
            IMessageProcessor messageProcessor,
            ILogger<TcpNetworkConnector> tcpNetworkConnectorLogger) : base(serviceRepository, mapper)
        {
            _serviceRepository = serviceRepository;
            NeuralmConfiguration neuralmConfiguration = neuralmConfigurationOptions.Value;
            _networkConnector = new TcpNetworkConnector(messageTypeCache, messageSerializer, messageProcessor, tcpNetworkConnectorLogger, neuralmConfiguration.Host, neuralmConfiguration.Port);
        }

        /// <inheritdoc cref="IService{TDto}.CreateAsync(TDto)"/>
        public override Task<(bool success, Guid id)> CreateAsync(ServiceDto dto)
        {
            return base.CreateAsync(dto)
                .ContinueWith(task =>
                {
                    if (task.Result.success && _networkConnector.IsRunning)
                    {
                        AddServiceCommand addServiceCommand = new AddServiceCommand()
                        {
                            Id = Guid.NewGuid(),
                            DateTime = dto.Start,
                            Service = new Messages.Dtos.ServiceDto()
                            {
                                Id = dto.Id,
                                Host = dto.Host,
                                Port = dto.Port,
                                Name = dto.Name
                            }
                        };
                        Task.Run(async () =>  await _networkConnector.SendMessageAsync(addServiceCommand, CancellationToken.None));
                    }  
                    return task.Result;
                });
        }

        /// <inheritdoc cref="IService{TDto}.DeleteAsync(TDto)"/>
        public override Task<(bool success, bool found)> DeleteAsync(ServiceDto dto)
        {
            return base.DeleteAsync(dto).ContinueWith(task =>
            {
                if (task.Result.success && _networkConnector.IsRunning)
                {
                    RemoveServiceCommand removeServiceCommand = new RemoveServiceCommand()
                    {
                        Id = Guid.NewGuid(),
                        DateTime = DateTime.Now,
                        ServiceId = dto.Id
                    };
                    Task.Run(async () =>  await _networkConnector.SendMessageAsync(removeServiceCommand, CancellationToken.None));
                }  
                return task.Result;
            });
        }

        /// <inheritdoc cref="IRegistryService.StartupAsync(CancellationToken)"/>
        public async Task StartupAsync(CancellationToken cancellationToken)
        {
            // Link cancellation tokens.
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMinutes(5));
            using CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cancellationTokenSource.Token);
            await _networkConnector.ConnectAsync(cancellationToken);
            _networkConnector.Start();
        }

        /// <inheritdoc cref="IRegistryService.GetServiceByNameAsync(string)"/>
        public async Task<ServiceDto> GetServiceByNameAsync(string serviceName)
        {
             IEnumerable<Service> services = await _serviceRepository.FindManyAsync(service => service.Name == serviceName);
             return Mapper.Map<ServiceDto>(services.Last());
        }
    }
}