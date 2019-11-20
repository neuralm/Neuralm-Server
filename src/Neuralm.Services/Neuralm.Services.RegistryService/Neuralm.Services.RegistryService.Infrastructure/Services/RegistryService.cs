using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Neuralm.Services.RegistryService.Infrastructure.Services
{
    /// <summary>
    /// Represents the <see cref="RegistryService"/> class.
    /// </summary>
    public class RegistryService : BaseService<Service, ServiceDto>, IRegistryService
    {
        private readonly IRepository<Service> _serviceRepository;
        private readonly IMessageTypeCache _messageTypeCache;
        private readonly IMessageSerializer _messageSerializer;
        private readonly IMessageProcessor _messageProcessor;
        private readonly NeuralmConfiguration _neuralmConfiguration;

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
            _messageTypeCache = messageTypeCache;
            _messageSerializer = messageSerializer;
            _messageProcessor = messageProcessor;
            _neuralmConfiguration = neuralmConfigurationOptions.Value;
        }

        /// <inheritdoc cref="IRegistryService.StartupAsync(CancellationToken)"/>
        public async Task StartupAsync(CancellationToken cancellationToken)
        {
            // Link cancellation tokens.
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMinutes(5));
            using CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cancellationTokenSource.Token);

            TcpNetworkConnector tcpNetworkConnector = new TcpNetworkConnector(_messageTypeCache, _messageSerializer, _messageProcessor, _neuralmConfiguration.Host, _neuralmConfiguration.Port);
            await tcpNetworkConnector.ConnectAsync(cancellationToken);
            tcpNetworkConnector.Start();
            
            List<Task> tasks = _neuralmConfiguration.Services.Select(serviceName => StartUpServiceTask(serviceName, cts.Token)).ToList();

            // foreach service in the configuration 
                // Check repository if service is alive
                    // if not, invoke a new instance of the service with a callback url for completion. 
                    // (use CancellationToken 5-10 min, if not returned try once more)
                        // if it failed to start up again abort and notify administrator.
            
            await Task.WhenAll(tasks);
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
            
        }
    }
}