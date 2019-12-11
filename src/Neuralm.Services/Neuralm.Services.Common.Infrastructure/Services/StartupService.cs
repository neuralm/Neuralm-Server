using Microsoft.Extensions.Options;
using Neuralm.Services.Common.Application.Interfaces;
using Neuralm.Services.Common.Configurations;
using Neuralm.Services.Common.Messages.Dtos;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Neuralm.Services.Common.Infrastructure.Services
{
    /// <summary>
    /// Represents the <see cref="StartupService"/> class.
    /// </summary>
    public class StartupService : IStartupService
    {
        private readonly IMessageSerializer _messageSerializer;
        private readonly IAccessTokenService _accessTokenService;
        private readonly ILogger<StartupService> _logger;
        private readonly RegistryServiceConfiguration _registryServiceConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="StartupService"/> class.
        /// </summary>
        /// <param name="messageSerializer">The message serializer.</param>
        /// <param name="registryServiceConfigurationOptions">The registry service configuration options.</param>
        /// <param name="accessTokenService">The access token service.</param>
        /// <param name="logger">The logger.</param>
        public StartupService(
            IMessageSerializer messageSerializer,
            IOptions<RegistryServiceConfiguration> registryServiceConfigurationOptions,
            IAccessTokenService accessTokenService,
            ILogger<StartupService> logger)
        {
            _registryServiceConfiguration = registryServiceConfigurationOptions.Value;
            _messageSerializer = messageSerializer;
            _accessTokenService = accessTokenService;
            _logger = logger;
        }
        
        /// <inheritdoc cref="IStartupService.RegisterServiceAsync"/>
        public async Task RegisterServiceAsync(string serviceName, string host, int port)
        {
            _logger.Log(LogLevel.Information, "REGISTERSERVICEASYNC STARTED");
            ServiceDto serviceDto = new ServiceDto()
            {
                Id = Guid.NewGuid(),
                Host = host,
                Port = port,
                Name = serviceName,
                Start = DateTime.Now
            };
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, serviceName),
                new Claim(ClaimTypes.Role, "Service")
            };
            using HttpClient httpClient = new HttpClient();
            string json = "";
            try
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_accessTokenService.GenerateAccessToken(claims)}");
                httpClient.BaseAddress = new Uri($"http://{_registryServiceConfiguration.Host}:{_registryServiceConfiguration.Port.ToString()}");
                json = _messageSerializer.SerializeToString(serviceDto);
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, $"RegistryServiceAsync: {e.Message}");
            }
            
            int attempt = 0;
            HttpResponseMessage response = null;
            _logger.Log(LogLevel.Information, "STARTING TO BROADCAST~!");
            do
            {
                try
                {
                    response = await httpClient.PostAsync("registry", new StringContent(json, Encoding.UTF8, "application/json"));
                    _logger.Log(LogLevel.Information, $"RegistryServiceAsync: {serviceName}, status: {response.StatusCode.ToString()}, content: {await response.Content.ReadAsStringAsync()}");
                }
                catch (Exception e)
                {
                    _logger.Log(LogLevel.Information, $"RequestUri: {httpClient.BaseAddress}registry");
                    _logger.Log(LogLevel.Error, $"RegistryServiceAsync: attempt: {attempt}, message: {e.Message}, service: {serviceName}");
                }

                if (response?.StatusCode != HttpStatusCode.Created)
                {
                    await Task.Delay(TimeSpan.FromSeconds(5));
                }
                
            } while (response?.StatusCode != HttpStatusCode.Created  && ++attempt < 15);

            if (response != null && response.IsSuccessStatusCode)
            {
                _logger.Log(LogLevel.Information, $"RegistryServiceAsync: attempt: {attempt}, message: Successfully registered, service: {serviceName}");
            }
            else
            {
                _logger.Log(LogLevel.Error, $"RegistryServiceAsync: attempt: {attempt}, message: FAILED to register, service: {serviceName}");
            }
        }
    }
}