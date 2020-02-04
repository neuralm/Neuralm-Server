using Microsoft.Extensions.Logging;
using Neuralm.Services.Common.Application.Interfaces;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Neuralm.Services.Common.Infrastructure.Networking
{
    /// <summary>
    /// Represents the <see cref="TcpNetworkConnector"/> class; an implementation of the abstract <see cref="BaseNetworkConnector"/> class.
    /// </summary>
    public class TcpNetworkConnector : BaseNetworkConnector
    {
        /// <summary>
        /// Gets the tcp client.
        /// </summary>
        protected TcpClient TcpClient { get; }

        /// <summary>
        /// Gets the host.
        /// </summary>
        protected string Host { get; }

        /// <summary>
        /// Gets the port.
        /// </summary>
        protected int Port { get; }

        /// <inheritdoc cref="BaseNetworkConnector.EndPoint"/>
        public override EndPoint EndPoint => TcpClient.Client.RemoteEndPoint;

        /// <inheritdoc cref="BaseNetworkConnector.IsConnected"/>
        public override bool IsConnected => TcpClient.Connected;

        /// <inheritdoc cref="BaseNetworkConnector.IsDataAvailable"/>
        protected override bool IsDataAvailable => TcpClient.Available > 0;

        /// <summary>
        /// Initializes an instance of the <see cref="TcpNetworkConnector"/> class as client.
        /// </summary>
        /// <param name="messageTypeCache">The message type cache.</param>
        /// <param name="messageSerializer">The message serializer.</param>
        /// <param name="messageProcessor">The message processor.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="host">The host string.</param>
        /// <param name="port">The port.</param>
        public TcpNetworkConnector(
            IMessageTypeCache messageTypeCache,
            IMessageSerializer messageSerializer,
            IMessageProcessor messageProcessor,
            ILogger<TcpNetworkConnector> logger,
            string host, int port) : base(messageTypeCache, messageSerializer, messageProcessor, logger)
        {
            Host = host;
            Port = port;
            TcpClient = new TcpClient { Client = { NoDelay = true } };
        }

        /// <summary>
        /// Initializes an instance of the <see cref="TcpNetworkConnector"/> class as server.
        /// </summary>
        /// <param name="messageTypeCache">The message type cache.</param>
        /// <param name="messageSerializer">The message serializer.</param>
        /// <param name="messageProcessor">The message processor.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="tcpClient">The tcp client.</param>
        public TcpNetworkConnector(
            IMessageTypeCache messageTypeCache,
            IMessageSerializer messageSerializer,
            IMessageProcessor messageProcessor,
            ILogger<TcpNetworkConnector> logger,
            TcpClient tcpClient) : base(messageTypeCache, messageSerializer, messageProcessor, logger)
        {
            TcpClient = tcpClient;
            TcpClient.Client.NoDelay = true;
            Stream = TcpClient.GetStream();
        }

        /// <inheritdoc cref="BaseNetworkConnector.ConnectAsync"/>
        public override async Task ConnectAsync(CancellationToken cancellationToken)
        {
            if (IsConnected)
                return;
            await TcpClient.ConnectAsync(Host, Port);
            Stream = TcpClient.GetStream();
        }

        /// <summary>
        /// Disposes the tcp client and suppresses the garbage collector.
        /// </summary>
        /// <param name="disposing">A boolean indicating whether to dispose.</param>
        protected override void Dispose(bool disposing)
        {
            TcpClient?.Dispose();
            base.Dispose(disposing);
        }
    }
}
