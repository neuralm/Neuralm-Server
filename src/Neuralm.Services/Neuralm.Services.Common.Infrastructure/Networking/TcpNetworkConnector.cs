﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Neuralm.Services.Common.Application.Interfaces;

namespace Neuralm.Services.Common.Infrastructure.Networking
{
    /// <summary>
    /// Represents the <see cref="TcpNetworkConnector"/> class; an implementation of the abstract <see cref="BaseNetworkConnector"/> class.
    /// </summary>
    public sealed class TcpNetworkConnector : BaseNetworkConnector
    {
        private readonly TcpClient _tcpClient;
        private readonly string _host;
        private readonly int _port;
        private NetworkStream _networkStream;

        /// <inheritdoc cref="BaseNetworkConnector.EndPoint"/>
        public override EndPoint EndPoint => _tcpClient.Client.RemoteEndPoint;

        /// <inheritdoc cref="BaseNetworkConnector.IsConnected"/>
        public override bool IsConnected => _tcpClient.Connected;

        /// <inheritdoc cref="BaseNetworkConnector.IsDataAvailable"/>
        protected override bool IsDataAvailable => _networkStream.DataAvailable;

        /// <summary>
        /// Initializes an instance of the <see cref="TcpNetworkConnector"/> class as client.
        /// </summary>
        /// <param name="messageTypeCache">The message type cache.</param>
        /// <param name="messageSerializer">The message serializer.</param>
        /// <param name="messageProcessor">The message processor.</param>
        /// <param name="host">The host string.</param>
        /// <param name="port">The port.</param>
        public TcpNetworkConnector(IMessageTypeCache messageTypeCache, IMessageSerializer messageSerializer, IMessageProcessor messageProcessor, string host, int port) : base(messageTypeCache, messageSerializer, messageProcessor)
        {
            _host = host;
            _port = port;
            _tcpClient = new TcpClient { Client = { NoDelay = true } };
        }

        /// <summary>
        /// Initializes an instance of the <see cref="TcpNetworkConnector"/> class as server.
        /// </summary>
        /// <param name="messageTypeCache">The message type cache.</param>
        /// <param name="messageSerializer">The message serializer.</param>
        /// <param name="messageProcessor">The message processor.</param>
        /// <param name="tcpClient">The tcp client.</param>
        public TcpNetworkConnector(IMessageTypeCache messageTypeCache, IMessageSerializer messageSerializer, IMessageProcessor messageProcessor, TcpClient tcpClient) : base(messageTypeCache, messageSerializer, messageProcessor)
        {
            _tcpClient = tcpClient;
            //IPEndPoint endPoint = (IPEndPoint)tcpClient.Client.RemoteEndPoint;
            //IPHostEntry hostEntry = Dns.GetHostEntry(endPoint.Address);
            //_host = hostEntry.HostName;
            //_port = endPoint.Port;
            _tcpClient.Client.NoDelay = true;
            _networkStream = _tcpClient.GetStream();
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
        protected override ValueTask<int> ReceivePacketAsync(Memory<byte> memory, CancellationToken cancellationToken)
        {
            return _networkStream.ReadAsync(memory, cancellationToken);
        }

        /// <inheritdoc cref="BaseNetworkConnector.SendPacketAsync"/>
        protected override ValueTask SendPacketAsync(ReadOnlyMemory<byte> packet, CancellationToken cancellationToken)
        {
            return _networkStream.WriteAsync(packet, cancellationToken);
        }

        /// <summary>
        /// Disposes the tcp client and suppresses the garbage collector.
        /// </summary>
        public new void Dispose()
        {
            _tcpClient?.Dispose();
            base.Dispose(true);
        }
    }
}
