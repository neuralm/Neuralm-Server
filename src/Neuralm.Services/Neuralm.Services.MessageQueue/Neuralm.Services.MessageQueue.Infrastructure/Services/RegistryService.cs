using Microsoft.Extensions.Options;
using Neuralm.Services.MessageQueue.Application.Configurations;
using Neuralm.Services.MessageQueue.Application.Interfaces;
using Neuralm.Services.MessageQueue.Infrastructure.Networking;
using Neuralm.Services.RegistryService.Messages;
using Neuralm.Services.RegistryService.Messages.Dtos;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Neuralm.Services.MessageQueue.Infrastructure.Services
{
    /// <summary>
    /// Represents the <see cref="RegistryService"/> class.
    /// </summary>
    public class RegistryService : IRegistryService
    {
        private readonly IMessageSerializer _messageSerializer;
        private readonly IServiceMessageProcessor _serviceMessageProcessor;
        private readonly RegistryServiceMessageProcessor _registryServiceMessageProcessor;
        private readonly IMessageToServiceMapper _messageToServiceMapper;
        private readonly TcpListener _tcpListener;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryService"/> class.
        /// </summary>
        /// <param name="registryConfigurationOptions">The registry configuration options.</param>
        /// <param name="messageSerializer">The message serializer.</param>
        /// <param name="serviceMessageProcessor"></param>
        /// <param name="registryServiceMessageProcessor"></param>
        /// <param name="messageToServiceMapper">The message to service mapper.</param>
        public RegistryService(
            IOptions<RegistryConfiguration> registryConfigurationOptions,
            IMessageSerializer messageSerializer,
            IServiceMessageProcessor serviceMessageProcessor,
            RegistryServiceMessageProcessor registryServiceMessageProcessor,
            IMessageToServiceMapper messageToServiceMapper)
        {
            _messageSerializer = messageSerializer;
            _serviceMessageProcessor = serviceMessageProcessor;
            _registryServiceMessageProcessor = registryServiceMessageProcessor;
            _messageToServiceMapper = messageToServiceMapper;
            RegistryConfiguration registryConfiguration = registryConfigurationOptions.Value;
            _tcpListener = new TcpListener(IPAddress.Any, registryConfiguration.Port);
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
                    INetworkConnector networkConnector = new TcpNetworkConnector(_messageSerializer, _registryServiceMessageProcessor, tcpClient);
                    networkConnector.Start();
                }, cancellationToken);
                Console.WriteLine("Accepted a new RegistryService!");
            }
        }

        /// <inheritdoc cref="IRegistryService.AddServices(AddServicesCommand)"/>
        public async Task AddServices(AddServicesCommand addServicesCommand)
        {
            foreach (ServiceDto service in addServicesCommand.Services)
            {
                INetworkConnector networkConnector = new TcpNetworkConnector(_messageSerializer, _serviceMessageProcessor, service.Host, service.Port);
                await networkConnector.ConnectAsync(CancellationToken.None);
                networkConnector.Start();
                _messageToServiceMapper.AddService(service.Id, service.Name, networkConnector);
            }
        }

        /// <inheritdoc cref="IRegistryService.AddService(AddServiceCommand)"/>
        public async Task AddService(AddServiceCommand addServiceCommand)
        {
            INetworkConnector networkConnector = new TcpNetworkConnector(_messageSerializer, _serviceMessageProcessor, addServiceCommand.Service.Host, addServiceCommand.Service.Port);
            await networkConnector.ConnectAsync(CancellationToken.None);
            networkConnector.Start();
            _messageToServiceMapper.AddService(addServiceCommand.Service.Id, addServiceCommand.Service.Name, networkConnector);
        }

        /// <inheritdoc cref="IRegistryService.RemoveService(RemoveServiceCommand)"/>
        public Task RemoveService(RemoveServiceCommand removeServiceCommand)
        {
            return Task.Run(() => _messageToServiceMapper.RemoveService(removeServiceCommand.ServiceId));
        }
    }
}
