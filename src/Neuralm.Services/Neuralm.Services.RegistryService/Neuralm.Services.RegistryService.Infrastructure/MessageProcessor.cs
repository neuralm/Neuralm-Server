using System;
using System.Threading.Tasks;
using Neuralm.Services.Common.Application.Interfaces;
using Neuralm.Services.Common.Messages.Interfaces;

namespace Neuralm.Services.RegistryService.Infrastructure
{
    /// <summary>
    /// Represents the <see cref="MessageProcessor"/> class.
    /// </summary>
    public class MessageProcessor : IMessageProcessor
    {
        /// <inheritdoc cref="IMessageProcessor.ProcessMessageAsync(IMessage, INetworkConnector)"/>
        public Task ProcessMessageAsync(IMessage message, INetworkConnector networkConnector)
        {
            Console.WriteLine($"Received a {message.GetType().FullName} in the message processor: {message}");
            return Task.CompletedTask;
        }
    }
}