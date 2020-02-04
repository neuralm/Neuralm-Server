using Microsoft.Extensions.Logging;
using Neuralm.Services.Common.Application.Interfaces;
using Neuralm.Services.Common.Domain;
using System;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Neuralm.Services.Common.Infrastructure.Networking
{
    /// <summary>
    /// Represents the <see cref="SslWSNetworkConnector"/> class.
    /// The secure version of the <see cref="WSNetworkConnector"/> class.
    /// Implemented according to https://tools.ietf.org/html/rfc6455.
    /// </summary>
    public class SslWSNetworkConnector : SslTcpNetworkConnector
    {
        private readonly IWSHandshakeHandler _wsHandshakeHandler;
        private WebSocket _webSocket;

        /// <summary>
        /// Initializes a new instance of the <see cref="SslWSNetworkConnector"/> class.
        /// </summary>
        /// <param name="messageTypeCache">The message type cache.</param>
        /// <param name="messageSerializer">The message serializer.</param>
        /// <param name="messageProcessor">The message processor.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="wsHandshakeHandler">The websocket handshake handler.</param>
        /// <param name="host">The host string.</param>
        /// <param name="port">The port.</param>
        public SslWSNetworkConnector(
            IMessageTypeCache messageTypeCache, 
            IMessageSerializer messageSerializer,
            IMessageProcessor messageProcessor, 
            ILogger<SslWSNetworkConnector> logger,
            IWSHandshakeHandler wsHandshakeHandler,
            string host, int port) : base(messageTypeCache, messageSerializer, messageProcessor, logger, host, port)
        {
            _wsHandshakeHandler = wsHandshakeHandler;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SslWSNetworkConnector"/> class.
        /// </summary>
        /// <param name="messageTypeCache">The message type cache.</param>
        /// <param name="messageSerializer">The message serializer.</param>
        /// <param name="messageProcessor">The message processor.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="wsHandshakeHandler">The websocket handshake handler.</param>
        /// <param name="tcpClient">The tcp client.</param>
        public SslWSNetworkConnector(
            IMessageTypeCache messageTypeCache, 
            IMessageSerializer messageSerializer,
            IMessageProcessor messageProcessor, 
            ILogger<SslWSNetworkConnector> logger,
            IWSHandshakeHandler wsHandshakeHandler,
            TcpClient tcpClient) : base(messageTypeCache, messageSerializer, messageProcessor, logger, tcpClient)
        {
            _wsHandshakeHandler = wsHandshakeHandler;
        }

        /// <summary>
        /// Starts the handshake as server asynchronously.
        /// </summary>
        /// <returns>Returns an awaitable <see cref="Task"/>.</returns>
        public async Task StartHandshakeAsServerAsync()
        {
            if (!IsConnected)
                throw new NetworkConnectorIsNotYetConnectedException("Call ConnectAsync first.");
            HandshakeResult handshakeResult = await _wsHandshakeHandler.HandleHandshakeAsServerAsync(Stream);
            if (!handshakeResult.Success)
            {
                Dispose();
                return;
            }

            _webSocket = handshakeResult.WebSocket;
        }

        /// <summary>
        /// Starts the handshake as client asynchronously.
        /// </summary>
        /// <returns>Returns an awaitable <see cref="Task"/>.</returns>
        public async Task StartHandshakeAsClientAsync()
        {
            if (!IsConnected)
                throw new NetworkConnectorIsNotYetConnectedException("Call ConnectAsync first.");
            HandshakeResult handshakeResult = await _wsHandshakeHandler.HandleHandshakeAsClientAsync(Stream, Host);

            if (!handshakeResult.Success)
            {
                Dispose();
                return;
            }

            _webSocket = handshakeResult.WebSocket;
        }

        /// <inheritdoc cref="BaseNetworkConnector.ReceivePacketAsync"/>
        protected override async ValueTask<int> ReceivePacketAsync(Memory<byte> memory, CancellationToken cancellationToken)
        {
            if (!_wsHandshakeHandler.HandshakeComplete)
                throw new HandshakeIsNotCompletedYetException("Call StartHandshakeAsClient/StartHandshakeAsServer first.");

            ValueWebSocketReceiveResult rec = await _webSocket.ReceiveAsync(memory, cancellationToken);
            return rec.Count;
        }

        /// <inheritdoc cref="BaseNetworkConnector.SendPacketAsync"/>
        protected override ValueTask SendPacketAsync(ReadOnlyMemory<byte> packet, CancellationToken cancellationToken)
        {
            if (!_wsHandshakeHandler.HandshakeComplete)
                throw new HandshakeIsNotCompletedYetException("Call StartHandshakeAsClient/StartHandshakeAsServer first.");

            return _webSocket.SendAsync(packet, WebSocketMessageType.Binary, true, cancellationToken);
        }

        /// <summary>
        /// Disposes the tcp client / websocket and suppresses the garbage collector.
        /// </summary>
        /// <param name="disposing">A boolean indicating whether to dispose.</param>
        protected override void Dispose(bool disposing)
        {
            _webSocket?.Dispose();
            base.Dispose(disposing);
        }
    }
}