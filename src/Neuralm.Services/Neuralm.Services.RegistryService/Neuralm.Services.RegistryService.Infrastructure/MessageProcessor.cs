using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Neuralm.Services.Common.Application.Interfaces;
using Neuralm.Services.Common.Messages.Interfaces;

namespace Neuralm.Services.RegistryService.Infrastructure
{
    /// <summary>
    /// Represents the <see cref="MessageProcessor"/> class.
    /// </summary>
    public class MessageProcessor : IMessageProcessor
    {
        private readonly ILogger<MessageProcessor> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageProcessor"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public MessageProcessor(ILogger<MessageProcessor> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc cref="IMessageProcessor.ProcessMessageAsync(IMessage, INetworkConnector)"/>
        public Task ProcessMessageAsync(IMessage message, INetworkConnector networkConnector)
        {
            _logger.LogInformation($"Received a {message.GetType().FullName} in the message processor: {message}");
            return Task.CompletedTask;
        }
    }
}