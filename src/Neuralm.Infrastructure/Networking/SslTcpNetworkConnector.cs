using System;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Neuralm.Application.Interfaces;
using Neuralm.Infrastructure.Interfaces;

namespace Neuralm.Infrastructure.Networking
{
    public sealed class SslTcpNetworkConnector : BaseNetworkConnector
    {
        private readonly TcpClient _tcpClient;
        private readonly string _host;
        private readonly int _port;
        private SslStream _sslStream;

        public override bool IsConnected => _tcpClient.Connected;
        protected override bool IsDataAvailable => _tcpClient.Available > 0;

        public SslTcpNetworkConnector(IMessageSerializer messageSerializer, IMessageProcessor messageProcessor, string host, int port) : base(messageSerializer, messageProcessor)
        {
            _tcpClient = new TcpClient();
            _host = host;
            _port = port;
        }

        public SslTcpNetworkConnector(IMessageSerializer messageSerializer, IMessageProcessor messageProcessor, TcpClient tcpClient) : base(messageSerializer, messageProcessor)
        {
            _tcpClient = tcpClient;
            SetSslStream();
        }

        public override async Task ConnectAsync(CancellationToken cancellationToken)
        {
            if (!IsConnected)
                return;
            await _tcpClient.ConnectAsync(_host, _port);
            SetSslStream();
        }

        private void SetSslStream()
        {
            _sslStream = new SslStream(_tcpClient.GetStream(), false);
        }

        public async Task AuthenticateAsServer(X509Certificate certificate)
        {
            await _sslStream.AuthenticateAsServerAsync(certificate, clientCertificateRequired: false, checkCertificateRevocation: true);
        }

        protected override ValueTask<int> SendPacketAsync(ReadOnlyMemory<byte> packet, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        protected override ValueTask<int> ReceivePacketAsync(Memory<byte> memory, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
