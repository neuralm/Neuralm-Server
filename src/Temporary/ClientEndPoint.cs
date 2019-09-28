using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Neuralm.Application.Configurations;
using Neuralm.Application.Interfaces;
using Neuralm.Infrastructure.Interfaces;
using Neuralm.Infrastructure.Networking;

namespace Neuralm.Infrastructure.EndPoints
{
    /// <summary>
    /// Represents the <see cref="ClientEndPoint"/> class.
    /// </summary>
    public class ClientEndPoint : IClientEndPoint
    {
        private readonly TcpListener _tcpListener;
        private readonly ServerConfiguration _serverConfiguration;

        /// <summary>
        /// Initializes an instance of the <see cref="ClientEndPoint"/> class.
        /// </summary>
        /// <param name="serverConfiguration">The server configuration.</param>
        public ClientEndPoint(IOptions<ServerConfiguration> serverConfiguration)
        {
            _serverConfiguration = serverConfiguration.Value;
            _tcpListener = new TcpListener(IPAddress.Any, _serverConfiguration.ClientPort);
        }

        /// <inheritdoc cref="IClientEndPoint.StartAsync(CancellationToken, IMessageProcessor, IMessageSerializer)"/>
        public async Task StartAsync(CancellationToken cancellationToken, IMessageProcessor messageProcessor, IMessageSerializer messageSerializer)
        {
            _tcpListener.Start();
            Console.WriteLine($"Started listening for clients on port: {_serverConfiguration.ClientPort}.");
            while (!cancellationToken.IsCancellationRequested)
            {
                TcpClient tcpClient = await _tcpListener.AcceptTcpClientAsync();
                Console.WriteLine($"CLIENT | New connection: {tcpClient.Client.RemoteEndPoint}");
                _ = Task.Run(async () =>
                {
                    SslTcpNetworkConnector networkConnector = new SslTcpNetworkConnector(messageSerializer, messageProcessor, tcpClient);
                    await networkConnector.AuthenticateAsServer(_serverConfiguration.Certificate, CancellationToken.None);
                    networkConnector.Start();
                }, cancellationToken);
            }
        }

        /// <inheritdoc cref="IClientEndPoint.StopAsync()"/>
        public Task StopAsync()
        {
            _tcpListener.Stop();
            return Task.CompletedTask;
        }
    }
}
