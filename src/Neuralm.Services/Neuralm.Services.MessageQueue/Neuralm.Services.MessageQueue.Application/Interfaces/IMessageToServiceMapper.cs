using System;
using System.Collections.Generic;
using Neuralm.Services.Common.Application.Interfaces;

namespace Neuralm.Services.MessageQueue.Application.Interfaces
{
    /// <summary>
    /// Represents the <see cref="IMessageToServiceMapper"/> interface.
    /// </summary>
    public interface IMessageToServiceMapper
    {
        /// <summary>
        /// Gets the message to service map.
        /// </summary>
        IReadOnlyDictionary<Type, IServiceConnector> MessageToServiceMap { get; }

        /// <summary>
        /// Adds a service.
        /// </summary>
        /// <param name="id">The service id.</param>
        /// <param name="name">The service name.</param>
        /// <param name="networkConnector">The network connector.</param>
        void AddService(Guid id, string name, INetworkConnector networkConnector);

        /// <summary>
        /// Removes a service.
        /// </summary>
        /// <param name="serviceId">The service id.</param>
        void RemoveService(Guid serviceId);
    }
}
