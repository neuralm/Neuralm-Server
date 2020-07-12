using Neuralm.Services.Common.Application.Interfaces;
using System;

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

        /// <summary>
        /// Adds a service health check listener.
        /// </summary>
        /// <param name="messageId">The message id.</param>
        /// <param name="serviceHealthCheckListener">The service health check listener.</param>
        void AddServiceHealthCheckMessageListener(Guid messageId, ServiceHealthCheckListener serviceHealthCheckListener);

        /// <summary>
        /// Removes a health check listener for the given message id.
        /// </summary>
        /// <param name="messageId">The message id.</param>
        void RemoveServiceHealthCheckMessageListener(Guid messageId);
    }
}
