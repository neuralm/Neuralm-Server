using System.Net.WebSockets;
using Neuralm.Services.Common.Application.Interfaces;

namespace Neuralm.Services.Common.Infrastructure.Networking
{
    /// <summary>
    /// Represents the <see cref="HandshakeResult"/> struct.
    /// Used for representing the result of a handshake using the <see cref="IWSHandshakeHandler"/> interface.
    /// </summary>
    public readonly struct HandshakeResult
    {
        /// <summary>
        /// Gets the resulting web socket from the handshake.
        /// </summary>
        public WebSocket WebSocket { get; }

        /// <summary>
        /// Gets a value indicating whether the handshake was successful.
        /// </summary>
        public bool Success { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HandshakeResult"/> struct.
        /// </summary>
        /// <param name="webSocket">The websocket.</param>
        /// <param name="success">The success flag.</param>
        public HandshakeResult(WebSocket webSocket, bool success)
        {
            WebSocket = webSocket;
            Success = success;
        }
    }
}
