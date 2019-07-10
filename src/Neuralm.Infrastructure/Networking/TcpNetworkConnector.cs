using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Neuralm.Application.Interfaces;
using Neuralm.Infrastructure.Interfaces;

namespace Neuralm.Infrastructure.Networking
{
    public sealed class TcpNetworkConnector : BaseNetworkConnector, IDisposable
    {
        private readonly TcpClient _tcpClient;
        private readonly string _host;
        private readonly int _port;
        public override bool IsConnected => _tcpClient.Connected;

        public TcpNetworkConnector(IMessageSerializer messageSerializer, IMessageProcessor messageProcessor, string host, int port) : base(messageSerializer, messageProcessor)
        {
            _host = host;
            _port = port;
            _tcpClient = new TcpClient();
        }
        public TcpNetworkConnector(IMessageSerializer messageSerializer, IMessageProcessor messageProcessor, TcpClient tcpClient) : base(messageSerializer, messageProcessor)
        {
            _tcpClient = tcpClient;
            IPEndPoint endPoint = (IPEndPoint)tcpClient.Client.RemoteEndPoint;
            IPHostEntry hostEntry = Dns.GetHostEntry(endPoint.Address);
            _host = hostEntry.HostName;
            _port = endPoint.Port;
        }

        public override ValueTask ConnectAsync(CancellationToken cancellationToken)
        {
            return IsConnected ? new ValueTask() : new ValueTask(Task.Run(async () => await _tcpClient.ConnectAsync(_host, _port), cancellationToken));
        }
        protected override Task<int> ReceivePacketAsync(Memory<byte> memory, CancellationToken cancellationToken)
        {
            if (!MemoryMarshal.TryGetArray(memory, out ArraySegment<byte> segment))
                throw new ArgumentException("Cannot get ArraySegment<byte> from Memory<byte> memory.");
            return _tcpClient.Client.ReceiveAsync(segment, SocketFlags.None);
        }
        protected override Task<int> SendPacketAsync(ReadOnlyMemory<byte> packet, CancellationToken cancellationToken)
        {
            if (!MemoryMarshal.TryGetArray(packet, out ArraySegment<byte> segment))
                throw new ArgumentException("Cannot get ArraySegment<byte> from ReadOnlyMemory<byte> packet.");
            return _tcpClient.Client.SendAsync(segment, SocketFlags.None);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _tcpClient?.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~TcpNetworkConnector()
        {
            Dispose(false);
        }
    }
}
