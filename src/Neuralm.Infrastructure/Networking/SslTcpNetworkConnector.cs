using System;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
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
            _sslStream = new SslStream(_tcpClient.GetStream(), false);
        }

        private static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
                return true;

            Console.WriteLine("Certificate error: {0}", sslPolicyErrors);

            // Do not allow this client to communicate with unauthenticated servers.
            return false;
        }

        public override async Task ConnectAsync(CancellationToken cancellationToken)
        {
            if (IsConnected)
                return;
            await _tcpClient.ConnectAsync(_host, _port);
            _sslStream = new SslStream(_tcpClient.GetStream(), false, ValidateServerCertificate, null);
        }

        public async Task AuthenticateAsClient(CancellationToken cancellationToken)
        {
            try
            {
                await _sslStream.AuthenticateAsClientAsync(_host);
            }
            catch (AuthenticationException e)
            {
                Console.WriteLine($"Authentication failed - closing the connection.!\n\t{e.Message}");
                Console.WriteLine("AuthenticateAsClient is cancelled.");
                _tcpClient.Close();
                Dispose();
                await Task.FromCanceled(cancellationToken);
            }
        }

        public async Task AuthenticateAsServer(X509Certificate certificate, CancellationToken cancellationToken)
        {
            try
            {
                await _sslStream.AuthenticateAsServerAsync(certificate, false, SslProtocols.Tls, false);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Authentication failed - closing the connection.!\n\t{e.Message}");
                Console.WriteLine($"AuthenticateAsServer is cancelled for: {_tcpClient.Client.RemoteEndPoint}.");
                _tcpClient.Close();
                Dispose();
                await Task.FromCanceled(cancellationToken);
                return;
            }
            
            DisplaySecurityLevel(_sslStream);
            DisplaySecurityServices(_sslStream);
            DisplayCertificateInformation(_sslStream);
            DisplayStreamProperties(_sslStream);
        }

        protected override async ValueTask<int> SendPacketAsync(ReadOnlyMemory<byte> packet, CancellationToken cancellationToken)
        {
            await _sslStream.WriteAsync(packet, cancellationToken);
            return packet.Length;
        }

        protected override ValueTask<int> ReceivePacketAsync(Memory<byte> memory, CancellationToken cancellationToken)
        {
            return _sslStream.ReadAsync(memory, cancellationToken);
        }

        private static void DisplaySecurityLevel(SslStream stream)
        {
            Console.WriteLine("Cipher: {0} strength {1}", stream.CipherAlgorithm, stream.CipherStrength);
            Console.WriteLine("Hash: {0} strength {1}", stream.HashAlgorithm, stream.HashStrength);
            Console.WriteLine("Key exchange: {0} strength {1}", stream.KeyExchangeAlgorithm, stream.KeyExchangeStrength);
            Console.WriteLine("Protocol: {0}", stream.SslProtocol);
        }
        private static void DisplaySecurityServices(SslStream stream)
        {
            Console.WriteLine("Is authenticated: {0} as server? {1}", stream.IsAuthenticated, stream.IsServer);
            Console.WriteLine("IsSigned: {0}", stream.IsSigned);
            Console.WriteLine("Is Encrypted: {0}", stream.IsEncrypted);
        }
        private static void DisplayStreamProperties(SslStream stream)
        {
            Console.WriteLine("Can read: {0}, write {1}", stream.CanRead, stream.CanWrite);
            Console.WriteLine("Can timeout: {0}", stream.CanTimeout);
        }
        private static void DisplayCertificateInformation(SslStream stream)
        {
            Console.WriteLine("Certificate revocation list checked: {0}", stream.CheckCertRevocationStatus);

            X509Certificate localCertificate = stream.LocalCertificate;
            if (stream.LocalCertificate != null)
            {
                Console.WriteLine("Local cert was issued to {0} and is valid from {1} until {2}.",
                    localCertificate.Subject,
                    localCertificate.GetEffectiveDateString(),
                    localCertificate.GetExpirationDateString());
            }
            else
            {
                Console.WriteLine("Local certificate is null.");
            }
            // Display the properties of the client's certificate.
            X509Certificate remoteCertificate = stream.RemoteCertificate;
            if (stream.RemoteCertificate != null)
            {
                Console.WriteLine("Remote cert was issued to {0} and is valid from {1} until {2}.",
                    remoteCertificate.Subject,
                    remoteCertificate.GetEffectiveDateString(),
                    remoteCertificate.GetExpirationDateString());
            }
            else
            {
                Console.WriteLine("Remote certificate is null.");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _tcpClient?.Dispose();
                _sslStream?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
