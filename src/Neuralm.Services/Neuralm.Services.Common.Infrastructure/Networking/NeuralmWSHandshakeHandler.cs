using Microsoft.Extensions.Logging;
using Neuralm.Services.Common.Application.Interfaces;
using System;
using System.IO;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Neuralm.Services.Common.Infrastructure.Networking
{
    /// <summary>
    /// Represents the <see cref="NeuralmWSHandshakeHandler"/> class.
    /// Used for websocket connections on the /neuralm protocol.
    /// </summary>
    public class NeuralmWSHandshakeHandler : IWSHandshakeHandler
    {
        // (a special GUID specified by RFC 6455)
        private const string SpecialGuid = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
        private readonly ILogger<NeuralmWSHandshakeHandler> _logger;

        /// <inheritdoc cref="IWSHandshakeHandler.HandshakeComplete"/>
        public bool HandshakeComplete { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NeuralmWSHandshakeHandler"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public NeuralmWSHandshakeHandler(ILogger<NeuralmWSHandshakeHandler> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc cref="IWSHandshakeHandler.HandleHandshakeAsServerAsync(Stream)"/>
        public async Task<HandshakeResult> HandleHandshakeAsServerAsync(Stream stream)
        {
            _logger.LogInformation("Started handshake as server.");
            string secWebsocketAccept = string.Empty;
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, bufferSize: 1, leaveOpen: true))
            {
                // Read client handshake "Request-Line" format.
                Regex secWebsocketKey = new Regex("Sec-WebSocket-Key: (.*)");
                string requestLine = await reader.ReadLineAsync();
                if (!requestLine.Equals("GET /neuralm HTTP/1.1"))
                {
                    _logger.LogError("Request-Line was not valid.");
                    return new HandshakeResult(null, false);
                }

                string line;
                do
                {
                    line = await reader.ReadLineAsync();
                    Match match = secWebsocketKey.Match(line);
                    if (!match.Success) continue;
                    byte[] buffer = Encoding.UTF8.GetBytes(match.Groups[1].Value.Trim() + SpecialGuid);
                    using SHA1 sha1 = SHA1.Create();
                    byte[] hash = sha1.ComputeHash(buffer);
                    secWebsocketAccept = Convert.ToBase64String(hash);
                }
                while (!string.IsNullOrEmpty(line));
            }

            if (string.IsNullOrEmpty(secWebsocketAccept))
            {
                _logger.LogError("Sec-WebSocket-Key is not found.");
                return new HandshakeResult(null, false);
            }

            await using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8, bufferSize: 1, leaveOpen: true))
            {
                // Write server handshake "Status-Line" format.
                await writer.WriteAsync($"HTTP/1.1 101 Switching Protocols\r\n");
                await writer.WriteAsync($"Connection: Upgrade\r\n");
                await writer.WriteAsync($"Upgrade: websocket\r\n");
                await writer.WriteAsync($"Sec-WebSocket-Accept: {secWebsocketAccept}\r\n");
                await writer.WriteAsync($"\r\n");
            }

            WebSocket webSocket = WebSocket.CreateFromStream(stream, true, "neuralm", Timeout.InfiniteTimeSpan);
            HandshakeComplete = true;
            _logger.LogInformation("Finished handshake as server.");
            return new HandshakeResult(webSocket, true);
        }

        /// <inheritdoc cref="IWSHandshakeHandler.HandleHandshakeAsClientAsync(Stream, string)"/>
        public async Task<HandshakeResult> HandleHandshakeAsClientAsync(Stream stream, string host)
        {
            _logger.LogInformation("Started handshake as client.");
            await using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8, bufferSize: 1, leaveOpen: true))
            {
                // Write client handshake "Request-Line" format.
                await writer.WriteAsync($"GET /neuralm HTTP/1.1\r\n");
                await writer.WriteAsync($"Host: {host}\r\n");
                await writer.WriteAsync($"Upgrade: websocket\r\n");
                await writer.WriteAsync($"Connection: Upgrade\r\n");
                await writer.WriteAsync($"Sec-WebSocket-Version: 13\r\n");
                await writer.WriteAsync($"Sec-WebSocket-Key: {Convert.ToBase64String(Guid.NewGuid().ToByteArray())}\r\n");
                await writer.WriteAsync($"\r\n");
            }

            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, bufferSize: 1, leaveOpen: true))
            {
                // Read server handshake "Status-Line" format.
                string statusLine = await reader.ReadLineAsync();
                if (!statusLine.Equals("HTTP/1.1 101 Switching Protocols"))
                {
                    _logger.LogError("Status-Line was not valid.");
                    return new HandshakeResult(null, false);
                }
                string remainder;
                do remainder = await reader.ReadLineAsync();
                while (!string.IsNullOrEmpty(remainder));
            }
            WebSocket webSocket = WebSocket.CreateFromStream(stream, false, "neuralm", Timeout.InfiniteTimeSpan);
            HandshakeComplete = true;
            _logger.LogInformation("Finished handshake as client.");
            return new HandshakeResult(webSocket, true);
        }
    }
}
