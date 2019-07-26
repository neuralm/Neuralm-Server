using Neuralm.Application.Exceptions;
using Neuralm.Application.Interfaces;
using Neuralm.Application.Messages;
using Neuralm.Infrastructure.Interfaces;
using System;
using System.Buffers;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Neuralm.Infrastructure.Networking
{
    /// <summary>
    /// Represents the <see cref="BaseNetworkConnector"/> class; an abstraction of the <see cref="INetworkConnector"/> interface.
    /// </summary>
    public abstract class BaseNetworkConnector : INetworkConnector
    {
        private readonly IMessageProcessor _messageProcessor;
        private readonly MessageConstructor _messageConstructor;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private int _minimumBufferSizeHint = 512;
        private const int AbsoluteMinimumBufferSizeHint = 512;

        /// <summary>
        /// Gets and sets a value indicating whether <see cref="Start"/> has ran.
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// Gets a value indicating whether <see cref="ConnectAsync"/> has ran.
        /// </summary>
        public abstract bool IsConnected { get; }

        /// <summary>
        /// Gets a value indicating whether data is available.
        /// </summary>
        protected abstract bool IsDataAvailable { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="BaseNetworkConnector"/> class.
        /// </summary>
        /// <remarks>
        /// Also loads the <see cref="MessageTypeCache"/> on the first <see cref="BaseNetworkConnector"/> instance creation.
        /// </remarks>
        /// <exception cref="ArgumentNullException">If the messageProcessor is null; an argument exception is thrown.</exception>
        /// <param name="messageSerializer">The message serializer.</param>
        /// <param name="messageProcessor">The message processor.</param>
        protected BaseNetworkConnector(IMessageSerializer messageSerializer, IMessageProcessor messageProcessor)
        {
            _messageConstructor = new MessageConstructor(messageSerializer);
            _messageProcessor = messageProcessor ?? throw new ArgumentNullException(nameof(messageProcessor));
            MessageTypeCache.LoadMessageTypeCache();
        }

        /// <summary>
        /// Starts the <see cref="StartReadingTask"/>, and sets <see cref="IsRunning"/> to <c>true</c>.
        /// </summary>
        /// <exception cref="NetworkConnectorIsNotYetConnectedException">If <see cref="IsConnected"/> is <c>false</c>.</exception>
        public void Start()
        {
            if (!IsConnected)
                throw new NetworkConnectorIsNotYetConnectedException("NetworkConnector has not connected yet. Call ConnectAsync() first.");

            Task.Run(StartReadingTask, _cancellationTokenSource.Token);
            IsRunning = true;
        }

        /// <summary>
        /// Stops the network connector and sets <see cref="IsRunning"/> to <c>false</c>.
        /// </summary>
        public void Stop()
        {
            _cancellationTokenSource.Cancel();
            IsRunning = false;
        }

        /// <summary>
        /// Sends a message asynchronously.
        /// </summary>
        /// <typeparam name="TMessage">The message type.</typeparam>
        /// <param name="message">The message.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="NetworkConnectorIsNotYetStartedException">If <see cref="IsRunning"/> is <c>false</c>.</exception>
        /// <returns>Returns a <see cref="Task"/> to await.</returns>
        public async Task SendMessageAsync<TMessage>(TMessage message, CancellationToken cancellationToken)
        {
            if (!IsRunning)
                throw new NetworkConnectorIsNotYetStartedException("NetworkConnector has not started yet. Call Start() first");

            Message constructedMessage = _messageConstructor.ConstructMessage(message);
            await SendPacketAsync(constructedMessage.Header, cancellationToken);
            await SendPacketAsync(constructedMessage.Body, cancellationToken);
        }

        /// <summary>
        /// Connect asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable <see cref="Task"/>.</returns>
        public abstract Task ConnectAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Send a packet asynchronously.
        /// </summary>
        /// <param name="packet">The packet.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable <see cref="ValueTask"/>.</returns>
        protected abstract ValueTask SendPacketAsync(ReadOnlyMemory<byte> packet, CancellationToken cancellationToken);

        /// <summary>
        /// Receive a packet asynchronously.
        /// </summary>
        /// <param name="memory">The memory.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable <see cref="ValueTask"/> with <see cref="int"/> as type parameter; with the bytes count received.</returns>
        protected abstract ValueTask<int> ReceivePacketAsync(Memory<byte> memory, CancellationToken cancellationToken);

        private async Task StartReadingTask()
        {
            MessageHeader? header = null;
            while (IsConnected)
            {
                byte[] readBuffer = ArrayPool<byte>.Shared.Rent(_minimumBufferSizeHint);
                int bytesReceived;
                do
                {
                    bytesReceived = await ReceivePacketAsync(readBuffer, _cancellationTokenSource.Token);
                    Debug.WriteLine($"Bytes received: {bytesReceived}");
                } while (IsDataAvailable && bytesReceived == 0);

                ReadOnlySequence<byte> buffer = new ReadOnlySequence<byte>(readBuffer.AsMemory(0, bytesReceived));
                ArrayPool<byte>.Shared.Return(readBuffer);

                if (!TryReadMessageHeader(ref header, ref buffer))
                    continue;

                // Increase read buffer for body size
                _minimumBufferSizeHint = header.Value.BodySize;
                if (!TryReadMessageBody(header, buffer, out byte[] bodyBufferSource, out Memory<byte> bodyBufferMemory))
                    continue;

                // Reset read buffer to minimum buffer size
                _minimumBufferSizeHint = AbsoluteMinimumBufferSizeHint;
                _ = ProcessMessageTask(_cancellationTokenSource.Token, header.Value.TypeName, bodyBufferMemory, bodyBufferSource);

                // Clear header
                header = null;
            }
        }
        private Task ProcessMessageTask(CancellationToken cancellationToken, string typeName, Memory<byte> bodyBufferMemory, byte[] bodyBufferSource)
        {
            return Task.Run(async () =>
            {
                if (MessageTypeCache.TryGetMessageType(typeName, out Type type))
                {
                    object obj = _messageConstructor.DeconstructMessageBody(bodyBufferMemory, type);

                    switch (obj)
                    {
                        case IResponse responseImplementer:
                            _ = _messageProcessor.ProcessResponse(type, responseImplementer, this);
                            break;
                        case IRequest requestImplementer:
                            {
                                IResponse response = await _messageProcessor.ProcessRequest(type, requestImplementer, this);
                                await SendMessageAsync(response, CancellationToken.None);
                                break;
                            }
                        case ICommand commandImplementer:
                            _ = _messageProcessor.ProcessCommand(type, commandImplementer, this);
                            break;
                        case IEvent eventImplementer:
                            _ = _messageProcessor.ProcessEvent(type, eventImplementer, this);
                            break;
                        default:
                            Console.WriteLine($"Received unsupported message of type: {type.FullName}");
                            break;
                    }
                }

                ArrayPool<byte>.Shared.Return(bodyBufferSource);
            }, cancellationToken);
        }
        private static bool TryReadMessageBody(MessageHeader? header, ReadOnlySequence<byte> buffer, out byte[] bodyBufferSource, out Memory<byte> bodyBufferMemory)
        {
            bodyBufferSource = ArrayPool<byte>.Shared.Rent(header.Value.BodySize);
            bodyBufferMemory = bodyBufferSource.AsMemory(0, header.Value.BodySize);
            if (TryCopyBodyBuffer(buffer, header.Value.BodySize, bodyBufferMemory.Span))
                return true;
            ArrayPool<byte>.Shared.Return(bodyBufferSource);
            return false;
        }
        private static bool TryReadMessageHeader(ref MessageHeader? header, ref ReadOnlySequence<byte> buffer)
        {
            if (!(header is null))
                return true;
            if (!MessageHeader.TryParseHeader(buffer, out header) || header is null)
                return false;
            buffer = buffer.Slice(header.Value.GetHeaderSize());
            return true;
        }
        private static bool TryCopyBodyBuffer(in ReadOnlySequence<byte> buffer, int bodySize, Span<byte> destination)
        {
            if (buffer.Length < bodySize)
                return false;
            buffer.Slice(buffer.Start, bodySize).CopyTo(destination);
            return true;
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _cancellationTokenSource?.Dispose();
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
