using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Neuralm.Services.Common.Application.Interfaces;
using Neuralm.Services.Common.Infrastructure.Networking;
using Neuralm.Services.MessageQueue.Application.Configurations;
using Neuralm.Services.MessageQueue.Application.Interfaces;
using Neuralm.Services.MessageQueue.Infrastructure.Messaging;
using Neuralm.Services.RegistryService.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using IRegistryService = Neuralm.Services.MessageQueue.Application.Interfaces.IRegistryService;

namespace Neuralm.Services.MessageQueue.Infrastructure.Services
{
    /// <summary>
    /// Represents the <see cref="RegistryService"/> class.
    /// </summary>
    public class RegistryService : IRegistryService
    {
        private readonly IMessageSerializer _messageSerializer;
        private readonly IServiceMessageProcessor _serviceMessageProcessor;
        private readonly IRegistryServiceMessageProcessor _registryServiceMessageProcessor;
        private readonly IMessageToServiceMapper _messageToServiceMapper;
        private readonly TcpListener _tcpListener;
        private readonly IMessageTypeCache _messageTypeCache;
        private readonly IAccessTokenService _accessTokenService;
        private readonly ILogger<RegistryService> _registryServiceLogger;
        private readonly ILogger<TcpNetworkConnector> _tcpNetworkConnectorLogger;
        private readonly ILogger<HttpNetworkConnector> _httpNetworkConnectorLogger;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryService"/> class.
        /// </summary>
        /// <param name="registryConfigurationOptions">The registry configuration options.</param>
        /// <param name="messageSerializer">The message serializer.</param>
        /// <param name="serviceMessageProcessor">The service message processor.</param>
        /// <param name="messageToServiceMapper">The message to service mapper.</param>
        /// <param name="messageTypeCache">The message type cache.</param>
        /// <param name="accessTokenService">The access token service.</param>
        /// <param name="registryServiceLogger">The registry service logger.</param>
        /// <param name="tcpNetworkConnectorLogger">The tcp network connector logger.</param>
        /// <param name="registryServiceMessageProcessorLogger">The registry service message processor logger.</param>
        /// <param name="httpNetworkConnectorLogger">The http network connector logger.</param>
        public RegistryService(
            IOptions<RegistryConfiguration> registryConfigurationOptions,
            IMessageSerializer messageSerializer,
            IServiceMessageProcessor serviceMessageProcessor,
            IMessageToServiceMapper messageToServiceMapper,
            IRegistryServiceMessageTypeCache messageTypeCache,
            IAccessTokenService accessTokenService,
            ILogger<RegistryService> registryServiceLogger,
            ILogger<TcpNetworkConnector> tcpNetworkConnectorLogger,
            ILogger<RegistryServiceMessageProcessor> registryServiceMessageProcessorLogger,
            ILogger<HttpNetworkConnector> httpNetworkConnectorLogger)
        {
            _messageSerializer = messageSerializer;
            _serviceMessageProcessor = serviceMessageProcessor;
            _registryServiceMessageProcessor = new RegistryServiceMessageProcessor(this, registryServiceMessageProcessorLogger);
            _messageToServiceMapper = messageToServiceMapper;
            RegistryConfiguration registryConfiguration = registryConfigurationOptions.Value;
            _tcpListener = new TcpListener(IPAddress.Any, registryConfiguration.Port);
            _messageTypeCache = messageTypeCache;
            _accessTokenService = accessTokenService;
            _registryServiceLogger = registryServiceLogger;
            _tcpNetworkConnectorLogger = tcpNetworkConnectorLogger;
            _httpNetworkConnectorLogger = httpNetworkConnectorLogger;
        }

        /// <inheritdoc cref="IRegistryService.StartReceivingServiceEndPointsAsync(CancellationToken)"/>
        public async Task StartReceivingServiceEndPointsAsync(CancellationToken cancellationToken)
        {
            _tcpListener.Start();
            while (!cancellationToken.IsCancellationRequested)
            {
                TcpClient tcpClient = await _tcpListener.AcceptTcpClientAsync();
                _ = Task.Run(() =>
                {
                    INetworkConnector networkConnector = new TcpNetworkConnector(_messageTypeCache, _messageSerializer, _registryServiceMessageProcessor, _tcpNetworkConnectorLogger, tcpClient);
                    networkConnector.Start();
                }, cancellationToken);
                _registryServiceLogger.LogInformation("Accepted a new RegistryService!");
            }
        }

        /// <inheritdoc cref="IRegistryService.AddServices(AddServicesCommand)"/>
        public Task AddServices(AddServicesCommand addServicesCommand)
        {
            IEnumerable<Task> tasks = addServicesCommand.Services
                .Select(service => Task.Run(() => AddService(service.Id, service.Name, service.Host, service.Port)));
            return Task.WhenAll(tasks);
        }

        /// <inheritdoc cref="IRegistryService.AddService(AddServiceCommand)"/>
        public Task AddService(AddServiceCommand addServiceCommand)
        {
            return AddService(addServiceCommand.Service.Id, addServiceCommand.Service.Name, 
                addServiceCommand.Service.Host, addServiceCommand.Service.Port);
        }

        private async Task AddService(Guid id, string name, string host, int port)
        {
            Uri baseUrl = new Uri($"http://{host}:{port.ToString()}/{name.ToLower().Replace("service", "")}");
            INetworkConnector networkConnector = new HttpNetworkConnector(_messageSerializer, _serviceMessageProcessor, baseUrl, _accessTokenService, _httpNetworkConnectorLogger);
            await networkConnector.ConnectAsync(CancellationToken.None);
            networkConnector.Start();
            _messageToServiceMapper.AddService(id, name, networkConnector);
        }

        /// <inheritdoc cref="IRegistryService.RemoveService(RemoveServiceCommand)"/>
        public Task RemoveService(RemoveServiceCommand removeServiceCommand)
        {
            return Task.Run(() => _messageToServiceMapper.RemoveService(removeServiceCommand.ServiceId));
        }
    }
}
