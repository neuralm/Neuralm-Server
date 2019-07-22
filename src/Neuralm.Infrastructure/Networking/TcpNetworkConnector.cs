using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Neuralm.Application.Interfaces;
using Neuralm.Infrastructure.Interfaces;

namespace Neuralm.Infrastructure.Networking
{
    /// <summary>
    /// Represents the <see cref="TcpNetworkConnector"/> class; an implementation of the abstract <see cref="BaseNetworkConnector"/> class.
    /// </summary>
    public sealed class TcpNetworkConnector : BaseNetworkConnector, IDisposable
    {
        private readonly TcpClient _tcpClient;
        private readonly string _host;
        private readonly int _port;

        /// <summary>
        /// Gets a value indicating whether the connector is connected.
        /// </summary>
        public override bool IsConnected => _tcpClient.Connected;

        /// <summary>
        /// Initializes an instance of the <see cref="TcpNetworkConnector"/> class.
        /// </summary>
        /// <param name="messageSerializer">The message serializer.</param>
        /// <param name="messageProcessor">The message processor.</param>
        /// <param name="host">The host string.</param>
        /// <param name="port">The port.</param>
        public TcpNetworkConnector(IMessageSerializer messageSerializer, IMessageProcessor messageProcessor, string host, int port) : base(messageSerializer, messageProcessor)
        {
            _host = host;
            _port = port;
            _tcpClient = new TcpClient {Client = {NoDelay = true}};
        }

        /// <summary>
        /// Initializes an instance of the <see cref="TcpNetworkConnector"/> class.
        /// </summary>
        /// <param name="messageSerializer">The message serializer.</param>
        /// <param name="messageProcessor">The message processor.</param>
        /// <param name="tcpClient">The tcp client.</param>
        public TcpNetworkConnector(IMessageSerializer messageSerializer, IMessageProcessor messageProcessor, TcpClient tcpClient) : base(messageSerializer, messageProcessor)
        {
            _tcpClient = tcpClient;
            IPEndPoint endPoint = (IPEndPoint)tcpClient.Client.RemoteEndPoint;
            IPHostEntry hostEntry = Dns.GetHostEntry(endPoint.Address);
            _host = hostEntry.HostName;
            _port = endPoint.Port;
            _tcpClient.Client.NoDelay = true;
        }

        /// <inheritdoc cref="BaseNetworkConnector.ConnectAsync"/>
        public override ValueTask ConnectAsync(CancellationToken cancellationToken)
        {
            return IsConnected ? new ValueTask() : new ValueTask(Task.Run(async () => await _tcpClient.ConnectAsync(_host, _port), cancellationToken));
        }

        /// <inheritdoc cref="BaseNetworkConnector.ReceivePacketAsync"/>
        protected override ValueTask<int> ReceivePacketAsync(Memory<byte> memory, CancellationToken cancellationToken)
        {
            return _tcpClient.Client.ReceiveAsync(memory, SocketFlags.None, cancellationToken);
        }

        /// <inheritdoc cref="BaseNetworkConnector.SendPacketAsync"/>
        protected override ValueTask<int> SendPacketAsync(ReadOnlyMemory<byte> packet, CancellationToken cancellationToken)
        {
            return _tcpClient.Client.SendAsync(packet, SocketFlags.None, cancellationToken);
        }

        /// <summary>
        /// Disposes the tcp client.
        /// </summary>
        /// <param name="disposing">The disposing flag.</param>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _tcpClient?.Dispose();
            }
        }

        /// <summary>
        /// Disposes the tcp client and suppresses the garbage collector.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Deconstructs the class.
        /// </summary>
        ~TcpNetworkConnector()
        {
            Dispose(false);
        }
    }
}
