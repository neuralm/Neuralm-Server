using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Neuralm.Services.Common.Application.Interfaces;
using Neuralm.Services.Common.Configurations;
using Neuralm.Services.Common.Messages.Dtos;

namespace Neuralm.Services.Common.Infrastructure.Services
{
    /// <summary>
    /// Represents the <see cref="StartupService"/> class.
    /// </summary>
    public class StartupService : IStartupService
    {
        private readonly IMessageSerializer _messageSerializer;
        private readonly IAccessTokenService _accessTokenService;
        private readonly RegistryServiceConfiguration _registryServiceConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="StartupService"/> class.
        /// </summary>
        /// <param name="messageSerializer">The message serializer.</param>
        /// <param name="registryServiceConfigurationOptions">The registry service configuration options.</param>
        /// <param name="accessTokenService">The access token service.</param>
        public StartupService(
            IMessageSerializer messageSerializer,
            IOptions<RegistryServiceConfiguration> registryServiceConfigurationOptions,
            IAccessTokenService accessTokenService)
        {
            _registryServiceConfiguration = registryServiceConfigurationOptions.Value;
            _messageSerializer = messageSerializer;
            _accessTokenService = accessTokenService;
        }
        
        /// <inheritdoc cref="IStartupService.RegisterServiceAsync"/>
        public async Task RegisterServiceAsync(string serviceName, string host, int port)
        {
            ServiceDto serviceDto = new ServiceDto()
            {
                Id = Guid.NewGuid(),
                Host = host,
                Port = port,
                Name = serviceName
            };
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, serviceName),
                new Claim(ClaimTypes.Role, "Service")
            };
            using HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_accessTokenService.GenerateAccessToken(claims)}");
            string json = _messageSerializer.SerializeToString(serviceDto);
            httpClient.BaseAddress = new Uri($"{_registryServiceConfiguration.Host}:{_registryServiceConfiguration.Port.ToString()}");
            await httpClient.PostAsync("registry/create", new StringContent(json, Encoding.UTF8, "application/json"));
        }
    }
}