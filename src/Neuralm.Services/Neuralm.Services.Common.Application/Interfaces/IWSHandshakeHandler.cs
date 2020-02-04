using Neuralm.Services.Common.Infrastructure.Networking;
using System.IO;
using System.Threading.Tasks;

namespace Neuralm.Services.Common.Application.Interfaces
{
    /// <summary>
    /// Represents the <see cref="IWSHandshakeHandler"/> interface.
    /// Used for websocket connections to handle handshakes.
    /// </summary>
    public interface IWSHandshakeHandler
    {
        /// <summary>
        /// Gets a boolean indicating whether the handshake has completed.
        /// </summary>
        bool HandshakeComplete { get; }

        /// <summary>
        /// Handles the handshake as the server and creates a websocket asynchronously.
        /// </summary>
        /// <param name="stream">The stream to perform the handshake on.</param>
        /// <returns>Returns an awaitable <see cref="Task"/> of type <see cref="HandshakeResult"/>.</returns>
        Task<HandshakeResult> HandleHandshakeAsServerAsync(Stream stream);

        /// <summary>
        /// Handles the handshake as the client and creates a websocket asynchronously.
        /// </summary>
        /// <param name="stream">The stream to perform the handshake on.</param>
        /// <param name="host">The host.</param>
        /// <returns>Returns an awaitable <see cref="Task"/> of type <see cref="HandshakeResult"/>.</returns>
        Task<HandshakeResult> HandleHandshakeAsClientAsync(Stream stream, string host);
    }
}
