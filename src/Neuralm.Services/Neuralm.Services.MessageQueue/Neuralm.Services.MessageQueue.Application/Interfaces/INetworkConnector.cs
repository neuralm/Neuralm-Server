using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Neuralm.Services.MessageQueue.Application.Interfaces
{
    /// <summary>
    /// Represents the <see cref="INetworkConnector"/> interface.
    /// </summary>
    public interface INetworkConnector : IDisposable
    {
        /// <summary>
        /// Gets the end point.
        /// </summary>
        EndPoint EndPoint { get; }

        /// <summary>
        /// Gets a value indicating whether the network connector is connected.
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// Gets a value indicating whether the network connector is running.
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// Connects the network connector asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable <see cref="Task"/>.</returns>
        Task ConnectAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Sends a message asynchronously.
        /// </summary>
        /// <typeparam name="TMessage">The message type.</typeparam>
        /// <param name="message">The message.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable <see cref="Task"/>.</returns>
        Task SendMessageAsync<TMessage>(TMessage message, CancellationToken cancellationToken);

        /// <summary>
        /// Starts the network connector.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops the network connector.
        /// </summary>
        void Stop();
    }
}
