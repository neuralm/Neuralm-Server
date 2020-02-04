using Microsoft.Extensions.Logging;
using Neuralm.Services.Common.Application.Interfaces;
using System;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace Neuralm.Services.Common.Infrastructure.Networking
{
    /// <summary>
    /// Represents the <see cref="SslTcpNetworkConnector"/> class; a secure implementation of the <see cref="TcpNetworkConnector"/> class.
    /// The <see cref="SslStream"/> version of the <see cref="TcpNetworkConnector"/>.
    /// </summary>
    public class SslTcpNetworkConnector : TcpNetworkConnector
    {
        /// <summary>
        /// Initializes an instance of the <see cref="SslTcpNetworkConnector"/> class.
        /// </summary>
        /// <param name="messageTypeCache">The message type cache.</param>
        /// <param name="messageSerializer">The message serializer.</param>
        /// <param name="messageProcessor">The message processor.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="host">The host string.</param>
        /// <param name="port">The port.</param>
        public SslTcpNetworkConnector(
            IMessageTypeCache messageTypeCache,
            IMessageSerializer messageSerializer,
            IMessageProcessor messageProcessor,
            ILogger<SslTcpNetworkConnector> logger,
            string host, int port) : base(messageTypeCache, messageSerializer, messageProcessor, logger, host, port)
        {

        }

        /// <summary>
        /// Initializes an instance of the <see cref="SslTcpNetworkConnector"/> class.
        /// </summary>
        /// <param name="messageTypeCache">The message type cache.</param>
        /// <param name="messageSerializer">The message serializer.</param>
        /// <param name="messageProcessor">The message processor.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="tcpClient">The tcp client.</param>
        public SslTcpNetworkConnector(
            IMessageTypeCache messageTypeCache,
            IMessageSerializer messageSerializer,
            IMessageProcessor messageProcessor,
            ILogger<SslTcpNetworkConnector> logger,
            TcpClient tcpClient) : base(messageTypeCache, messageSerializer, messageProcessor, logger, tcpClient)
        {
            Stream = new SslStream(tcpClient.GetStream(), false);
        }

        /// <inheritdoc cref="BaseNetworkConnector.ConnectAsync"/>
        public override async Task ConnectAsync(CancellationToken cancellationToken)
        {
            if (IsConnected)
                return;
            await TcpClient.ConnectAsync(Host, Port);
            Stream = new SslStream(TcpClient.GetStream(), false, ValidateServerCertificate, null);
        }

        /// <summary>
        /// Authenticates as client.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable <see cref="Task"/>.</returns>
        public async Task AuthenticateAsClient(CancellationToken cancellationToken)
        {
            try
            {
                await ((SslStream) Stream).AuthenticateAsClientAsync(Host);
            }
            catch (AuthenticationException e)
            {
                Logger.LogError($"Authentication failed - closing the connection!\n\t{e.Message}");
                Logger.LogError("AuthenticateAsClient is cancelled.");
                TcpClient.Close();
                Dispose();
                await Task.FromCanceled(cancellationToken);
            }
        }

        /// <summary>
        /// Authenticates as server.
        /// </summary>
        /// <param name="certificate">The certificate.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable <see cref="Task"/>.</returns>
        public async Task AuthenticateAsServer(X509Certificate certificate, CancellationToken cancellationToken)
        {
            try
            {
                await ((SslStream) Stream).AuthenticateAsServerAsync(certificate, false, SslProtocols.Tls12, false);
            }
            catch (Exception e)
            {
                Logger.LogError($"Authentication failed - closing the connection!\n\t{e.Message}");
                Logger.LogError($"AuthenticateAsServer is cancelled for: {TcpClient.Client.RemoteEndPoint}.");
                TcpClient.Close();
                Dispose();
                await Task.FromCanceled(cancellationToken);
            }
        }

        /// <summary>
        /// Validates server certificate.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="certificate">The certificate.</param>
        /// <param name="chain">The x509 chain.</param>
        /// <param name="sslPolicyErrors">The ssl policy errors.</param>
        /// <returns>Returns <c>true</c> if <paramref name="sslPolicyErrors"/> equals <see cref="SslPolicyErrors.None"/>; otherwise, <c>false</c>.</returns>
        private bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
                return true;

            Logger.LogError("Certificate error: {0}", sslPolicyErrors);

            // Do not allow this client to communicate with unauthenticated servers.
            return false;
        }
    }
}
