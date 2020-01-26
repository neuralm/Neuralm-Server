using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Neuralm.Services.Common.Application.Interfaces;
using Neuralm.Services.Common.Domain;

namespace Neuralm.Services.Common.Infrastructure.Networking
{
    /// <summary>
    /// Represents the <see cref="WSNetworkConnector"/> class.
    /// Implemented according to https://tools.ietf.org/html/rfc6455.
    /// </summary>
    public class WSNetworkConnector : BaseNetworkConnector
    {
        // (a special GUID specified by RFC 6455)
        private const string SpecialGuid = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
        private readonly TcpClient _tcpClient;
        private WebSocket _webSocket;
        private NetworkStream _networkStream;
        private readonly string _host;
        private readonly int _port;
        private bool _handshakeComplete = false;

        /// <inheritdoc cref="BaseNetworkConnector.EndPoint"/>
        public override EndPoint EndPoint => _tcpClient.Client.RemoteEndPoint;

        /// <inheritdoc cref="BaseNetworkConnector.IsConnected"/>
        public override bool IsConnected => _tcpClient.Connected;

        /// <inheritdoc cref="BaseNetworkConnector.IsDataAvailable"/>
        protected override bool IsDataAvailable => _networkStream.DataAvailable;

        /// <summary>
        /// Initializes a new instance of the <see cref="WSNetworkConnector"/> class.
        /// </summary>
        /// <param name="messageTypeCache">The message type cache.</param>
        /// <param name="messageSerializer">The message serializer.</param>
        /// <param name="messageProcessor">The message processor.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="host">The host string.</param>
        /// <param name="port">The port.</param>
        public WSNetworkConnector(
            IMessageTypeCache messageTypeCache,
            IMessageSerializer messageSerializer,
            IMessageProcessor messageProcessor,
            ILogger<WSNetworkConnector> logger,
            string host, int port) : base(messageTypeCache, messageSerializer, messageProcessor, logger)
        {
            _host = host;
            _port = port;
            _tcpClient = new TcpClient { Client = { NoDelay = true } };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WSNetworkConnector"/> class.
        /// </summary>
        /// <param name="messageTypeCache">The message type cache.</param>
        /// <param name="messageSerializer">The message serializer.</param>
        /// <param name="messageProcessor">The message processor.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="tcpClient">The tcp client.</param>
        public WSNetworkConnector(
            IMessageTypeCache messageTypeCache,
            IMessageSerializer messageSerializer,
            IMessageProcessor messageProcessor,
            ILogger<WSNetworkConnector> logger, 
            TcpClient tcpClient) : base(messageTypeCache, messageSerializer, messageProcessor, logger)
        {
            _tcpClient = tcpClient;
            _tcpClient.Client.NoDelay = true;
            _networkStream = _tcpClient.GetStream();
        }

        /// <summary>
        /// Starts the handshake as server asynchronously.
        /// </summary>
        /// <returns>Returns an awaitable <see cref="Task"/>.</returns>
        public async Task StartHandshakeAsServerAsync()
        {
            if (!IsConnected)
                throw new NetworkConnectorIsNotYetConnectedException("Call ConnectAsync first.");
            Logger.LogInformation("Started handshake as server.");
            string secWebsocketAccept = string.Empty;
            using (StreamReader reader = new StreamReader(_networkStream, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, bufferSize: 1, leaveOpen: true))
            {
                // Read client handshake "Request-Line" format.
                Regex secWebsocketKey = new Regex("Sec-WebSocket-Key: (.*)");
                string requestLine = await reader.ReadLineAsync();
                if (!requestLine.Equals("GET /neuralm HTTP/1.1"))
                {
                    Logger.LogError("Request-Line was not valid.");
                    Dispose();
                    return;
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
                Logger.LogError("Sec-WebSocket-Key is not found.");
                Dispose();
                return;
            }

            await using (StreamWriter writer = new StreamWriter(_networkStream, Encoding.UTF8, bufferSize: 1, leaveOpen: true))
            {
                // Write server handshake "Status-Line" format.
                await writer.WriteAsync($"HTTP/1.1 101 Switching Protocols\r\n");
                await writer.WriteAsync($"Connection: Upgrade\r\n");
                await writer.WriteAsync($"Upgrade: websocket\r\n");
                await writer.WriteAsync($"Sec-WebSocket-Accept: {secWebsocketAccept}\r\n");
                await writer.WriteAsync($"\r\n");
            }

            _webSocket = WebSocket.CreateFromStream(_networkStream, true, "neuralm", Timeout.InfiniteTimeSpan);
            _handshakeComplete = true;
            Logger.LogInformation("Finished handshake as server.");
        }

        /// <summary>
        /// Starts the handshake as client asynchronously.
        /// </summary>
        /// <returns>Returns an awaitable <see cref="Task"/>.</returns>
        public async Task StartHandshakeAsClientAsync()
        {
            if (!IsConnected)
                throw new NetworkConnectorIsNotYetConnectedException("Call ConnectAsync first.");

            Logger.LogInformation("Started handshake as client.");
            await using (StreamWriter writer = new StreamWriter(_networkStream, Encoding.UTF8, bufferSize: 1, leaveOpen: true))
            {
                // Write client handshake "Request-Line" format.
                await writer.WriteAsync($"GET /neuralm HTTP/1.1\r\n");
                await writer.WriteAsync($"Host: {_host}\r\n");
                await writer.WriteAsync($"Upgrade: websocket\r\n");
                await writer.WriteAsync($"Connection: Upgrade\r\n");
                await writer.WriteAsync($"Sec-WebSocket-Version: 13\r\n");
                await writer.WriteAsync($"Sec-WebSocket-Key: {Convert.ToBase64String(Guid.NewGuid().ToByteArray())}\r\n");
                await writer.WriteAsync($"\r\n");
            }

            using (StreamReader reader = new StreamReader(_networkStream, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, bufferSize: 1, leaveOpen: true))
            {
                // Read server handshake "Status-Line" format.
                string statusLine = await reader.ReadLineAsync();
                if (!statusLine.Equals("HTTP/1.1 101 Switching Protocols"))
                {
                    Logger.LogError("Status-Line was not valid.");
                    Dispose();
                    return;
                }
                string remainder;
                do remainder = await reader.ReadLineAsync();
                while (!string.IsNullOrEmpty(remainder));
            }
            _webSocket = WebSocket.CreateFromStream(_networkStream, false, "neuralm", Timeout.InfiniteTimeSpan);
            _handshakeComplete = true;
            Logger.LogInformation("Finished handshake as client.");
        }

        /// <inheritdoc cref="BaseNetworkConnector.ConnectAsync"/>
        public override async Task ConnectAsync(CancellationToken cancellationToken)
        {
            if (IsConnected)
                return;
            await _tcpClient.ConnectAsync(_host, _port);
            _networkStream = _tcpClient.GetStream();
        }

        /// <inheritdoc cref="BaseNetworkConnector.ReceivePacketAsync"/>
        protected override async ValueTask<int> ReceivePacketAsync(Memory<byte> memory, CancellationToken cancellationToken)
        {
            if (!_handshakeComplete)
                throw new HandshakeIsNotCompletedYetException("Call StartHandshakeAsClient/StartHandshakeAsServer first.");

            ValueWebSocketReceiveResult rec = await _webSocket.ReceiveAsync(memory, cancellationToken);
            return rec.Count;
        }

        /// <inheritdoc cref="BaseNetworkConnector.SendPacketAsync"/>
        protected override ValueTask SendPacketAsync(ReadOnlyMemory<byte> packet, CancellationToken cancellationToken)
        {
            if (!_handshakeComplete)
                throw new HandshakeIsNotCompletedYetException("Call StartHandshakeAsClient/StartHandshakeAsServer first.");

            return _webSocket.SendAsync(packet, WebSocketMessageType.Binary, true, cancellationToken);
        }

        /// <summary>
        /// Disposes the tcp client / websocket and suppresses the garbage collector.
        /// </summary>
        public new void Dispose()
        {
            _tcpClient?.Dispose();
            _webSocket?.Dispose();
            base.Dispose(true);
        }
    }
}
