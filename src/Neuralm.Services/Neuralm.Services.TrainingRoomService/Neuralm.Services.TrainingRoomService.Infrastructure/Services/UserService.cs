using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Neuralm.Services.Common.Application.Interfaces;
using Neuralm.Services.TrainingRoomService.Application.Interfaces;
using Neuralm.Services.TrainingRoomService.Messages.Dtos;

namespace Neuralm.Services.TrainingRoomService.Infrastructure.Services
{
    /// <summary>
    /// Represents the <see cref="UserService"/> class.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IMessageSerializer _messageSerializer;
        private readonly HttpClient _httpClient;
        private readonly ILogger<UserService> _logger;

        public UserService(
            IMessageSerializer messageSerializer,
            HttpClient httpClient,
            ILogger<UserService> logger)
        {
            _messageSerializer = messageSerializer;
            _httpClient = httpClient;
            _logger = logger;
        }
        
        /// <inheritdoc cref="IUserService.FindUserAsync(Guid)"/>
        public async Task<UserDto> FindUserAsync(Guid id)
        {
            HttpResponseMessage responseMessage = await _httpClient.GetAsync($"user/{id}");
            if (!responseMessage.IsSuccessStatusCode) 
                return null;
            _logger.LogInformation($"[RESPONSE] [FindUserAsync] Response: {await responseMessage.Content.ReadAsStringAsync()}");
            byte[] bytes = await responseMessage.Content.ReadAsByteArrayAsync();
            UserDto user = _messageSerializer.Deserialize<UserDto>(bytes);
            return user;
        }
    }
}