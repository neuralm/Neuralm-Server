using Neuralm.Services.Common.Application.Interfaces;
using Neuralm.Services.Common.Messages.Dtos;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Neuralm.Services.Common.Infrastructure.Services
{
    /// <summary>
    /// Represents the <see cref="StartupService"/> class.
    /// </summary>
    public class StartupService : IStartupService
    {
        private readonly IRegistryService _registryService;
        private readonly ILogger<StartupService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="StartupService"/> class.
        /// </summary>
        /// <param name="registryService">The registry service.</param>
        /// <param name="logger">The logger.</param>
        public StartupService(
            IRegistryService registryService,
            ILogger<StartupService> logger)
        {
            _registryService = registryService;
            _logger = logger;
        }
        
        /// <inheritdoc cref="IStartupService.RegisterServiceAsync"/>
        public async Task RegisterServiceAsync(string serviceName, string host, int port)
        {
            _logger.LogInformation("[STARTED] [RegisterServiceAsync]");
            ServiceDto serviceDto = new ServiceDto()
            {
                Id = Guid.NewGuid(),
                Host = host,
                Port = port,
                Name = serviceName,
                Start = DateTime.Now
            };

            int attempt = 0;
            _logger.LogInformation("[BROADCASTING] [RegisterServiceAsync]");
            bool result = false;
            do
            {
                result = await _registryService.AddServiceAsync(serviceDto);
                _logger.LogInformation($"[BROADCASTING] [RegistryServiceAsync] attempt: {attempt}, Success: {result}, service: {serviceName}");

                if (result) 
                    continue;
                
                await Task.Delay(TimeSpan.FromSeconds(5));
                _logger.LogInformation($"[DELAY] [RegistryServiceAsync] Delaying request 5 seconds.");
            } while (!result && ++attempt < 15);

            if (result)
            {
                _logger.LogInformation($"[FINISHED] [RegistryServiceAsync] attempt: {attempt}, message: Successfully registered, service: {serviceName}");
            }
            else
            {
                _logger.LogError($"RegistryServiceAsync: attempt: {attempt}, message: FAILED to register, service: {serviceName}");
            }
        }
    }
}