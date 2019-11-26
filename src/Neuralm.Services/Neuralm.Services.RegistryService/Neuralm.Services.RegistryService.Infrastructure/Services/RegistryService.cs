using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Options;
using Neuralm.Services.Common.Application.Abstractions;
using Neuralm.Services.Common.Application.Interfaces;
using Neuralm.Services.Common.Infrastructure.Networking;
using Neuralm.Services.RegistryService.Application.Configurations;
using Neuralm.Services.RegistryService.Application.Dtos;
using Neuralm.Services.RegistryService.Application.Interfaces;
using Neuralm.Services.RegistryService.Domain;
using Neuralm.Services.RegistryService.Messages;

namespace Neuralm.Services.RegistryService.Infrastructure.Services
{
    /// <summary>
    /// Represents the <see cref="RegistryService"/> class.
    /// </summary>
    public class RegistryService : BaseService<Service, ServiceDto>, IRegistryService
    {
        private readonly IRepository<Service> _serviceRepository;
        private readonly NeuralmConfiguration _neuralmConfiguration;
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
        public RegistryService(
            IRepository<Service> serviceRepository, 
            IMapper mapper,
            IOptions<NeuralmConfiguration> neuralmConfigurationOptions,
            IMessageTypeCache messageTypeCache,
            IMessageSerializer messageSerializer,
            IMessageProcessor messageProcessor) : base(serviceRepository, mapper)
        {
            _serviceRepository = serviceRepository;
            _neuralmConfiguration = neuralmConfigurationOptions.Value;
            _networkConnector = new TcpNetworkConnector(messageTypeCache, messageSerializer, messageProcessor, _neuralmConfiguration.Host, _neuralmConfiguration.Port);
        }

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
                                Name = dto.Name,
                            }
                        };
                        Task.Run(async () =>  await _networkConnector.SendMessageAsync(addServiceCommand, CancellationToken.None));
                    }  
                    return task.Result;
                });
        }

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

//            List<Task> tasks = _neuralmConfiguration.Services.Select(serviceName => StartUpServiceTask(serviceName, cts.Token)).ToList();
//
//            // foreach service in the configuration 
//                // Check repository if service is alive
//                    // if not, invoke a new instance of the service with a callback url for completion. 
//                    // (use CancellationToken 5-10 min, if not returned try once more)
//                        // if it failed to start up again abort and notify administrator.
//            
//            await Task.WhenAll(tasks);
        }

        /// <inheritdoc cref="IRegistryService.StartMonitoringAsync(CancellationToken)"/>
        public async Task StartMonitoringAsync(CancellationToken cancellationToken)
        {
            // TODO: Pulse each service
            throw new System.NotImplementedException();
        }

        private async Task StartUpServiceTask(string serviceName, CancellationToken cancellationToken)
        {
            // Check if a service currently exists that is alive
            if (await _serviceRepository.ExistsAsync(service => service.IsAlive && service.Name == serviceName))
                return;
            cancellationToken.ThrowIfCancellationRequested();
            throw new System.NotImplementedException();
        }
    }
}