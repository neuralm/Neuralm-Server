using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Neuralm.Services.Common.Application.Interfaces;
using Neuralm.Services.Common.Messages.Dtos;

namespace Neuralm.Services.Common.Infrastructure.Services
{
    /// <summary>
    /// Represents the <see cref="RegistryService"/> class.
    /// </summary>
    public class RegistryService : IRegistryService
    {
        private readonly IMessageSerializer _messageSerializer;
        private readonly ILogger<RegistryService> _logger;
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryService"/> class.
        /// </summary>
        /// <param name="messageSerializer">The message serializer.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="httpClient">The http client.</param>
        public RegistryService(
            IMessageSerializer messageSerializer,
            ILogger<RegistryService> logger,
            HttpClient httpClient)
        {
            _messageSerializer = messageSerializer;
            _logger = logger;
            _httpClient = httpClient;
        }
        
        /// <inheritdoc cref="IRegistryService.AddServiceAsync(ServiceDto)"/>
        public async Task<bool> AddServiceAsync(ServiceDto service)
        {
            _logger.LogInformation("[STARTED] [AddServiceAsync]");
            try
            {
                string json = _messageSerializer.SerializeToString(service);
                StringContent stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                _logger.LogInformation($"[CONTENT] [AddServiceAsync] StringContent is:  {json}");
                HttpResponseMessage responseMessage = await _httpClient.PostAsync("registry", stringContent);
                _logger.LogInformation($"[REQUEST] [AddServiceAsync] Response status code: {responseMessage.StatusCode.ToString()}");
                return responseMessage.IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                _logger.LogError($"[ERROR] [AddServiceAsync] {e.Message}");
                return false;
            }
            finally
            {
                _logger.LogInformation("[FINISHED] [AddServiceAsync]");
            }
        }

        /// <inheritdoc cref="IRegistryService.GetServiceAsync(string)"/>
        public Task<ServiceDto> GetServiceAsync(string serviceName)
        {
            throw new System.NotImplementedException();
        }
    }
}