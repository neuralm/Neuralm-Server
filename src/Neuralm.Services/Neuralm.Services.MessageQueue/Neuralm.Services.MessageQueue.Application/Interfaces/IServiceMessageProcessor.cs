using System;
using Neuralm.Services.Common.Application.Interfaces;

namespace Neuralm.Services.MessageQueue.Application.Interfaces
{
    /// <summary>
    /// Represents the <see cref="IServiceMessageProcessor"/> interface.
    /// </summary>
    public interface IServiceMessageProcessor : IMessageProcessor
    {
        /// <summary>
        /// Adds a client message id with network connector to the message dictionary.
        /// </summary>
        /// <param name="messageId">The message id.</param>
        /// <param name="networkConnector">The network connector.</param>
        void AddClientMessage(Guid messageId, INetworkConnector networkConnector);
    }
}
