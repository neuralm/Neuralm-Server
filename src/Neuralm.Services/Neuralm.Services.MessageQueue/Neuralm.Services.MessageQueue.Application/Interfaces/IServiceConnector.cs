using Neuralm.Services.Common.Messages.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Neuralm.Services.MessageQueue.Application.Interfaces
{
    /// <summary>
    /// Represent the <see cref="IServiceConnector"/> interface.
    /// </summary>
    public interface IServiceConnector
    {
        /// <summary>
        /// Gets the name of the service.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Enqueue message in the service message queue.
        /// </summary>
        /// <param name="message">The message.</param>
        void EnqueueMessage(IMessage message);

        /// <summary>
        /// Starts publishing messages from the message queue asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable <see cref="Task"/>.</returns>
        Task StartPublishingAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Adds a service to the service network connector dictionary.
        /// </summary>
        /// <param name="networkConnector">The network connector.</param>
        /// <param name="id">The service id.</param>
        void AddService(INetworkConnector networkConnector, Guid id);

        /// <summary>
        /// Removes a service from the service connector dictionary by id.
        /// </summary>
        /// <param name="serviceId"></param>
        void RemoveService(Guid serviceId);
    }
}
