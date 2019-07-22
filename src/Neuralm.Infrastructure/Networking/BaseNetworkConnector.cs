using Neuralm.Application.Interfaces;
using System;
using System.Buffers;
using System.Diagnostics;
using System.IO.Pipelines;
using System.Threading;
using System.Threading.Tasks;
using Neuralm.Application.Exceptions;
using Neuralm.Application.Messages;
using Neuralm.Infrastructure.Interfaces;

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
        private int _expectingHeader = 1;
        private int _bytesReceived = 0;

        /// <summary>
        /// Gets and sets a value indicating whether <see cref="Start"/> has ran.
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// Gets a value indicating whether <see cref="ConnectAsync"/> has ran.
        /// </summary>
        public abstract bool IsConnected { get; }

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
        /// Starts the <see cref="StartReadingTask"/> and <see cref="StartWritingTask"/>, and sets <see cref="IsRunning"/> to <c>true</c>.
        /// </summary>
        /// <exception cref="NetworkConnectorIsNotYetConnectedException">If <see cref="IsConnected"/> is <c>false</c>.</exception>
        public void Start()
        {
            if (!IsConnected)
                throw new NetworkConnectorIsNotYetConnectedException("NetworkConnector has not connected yet. Call ConnectAsync() first.");

            Pipe pipe = new Pipe();

            _ = StartWritingTask(pipe.Writer);
            _ = StartReadingTask(pipe.Reader);

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
        /// Sends a message through the pipeline asynchronously.
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
        /// <returns>Returns an awaitable <see cref="ValueTask"/>.</returns>
        public abstract ValueTask ConnectAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Send a packet asynchronously.
        /// </summary>
        /// <param name="packet">The packet.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable <see cref="ValueTask"/> with <see cref="int"/> as type parameter; with the bytes count sent.</returns>
        protected abstract ValueTask<int> SendPacketAsync(ReadOnlyMemory<byte> packet, CancellationToken cancellationToken);

        /// <summary>
        /// Receive a packet asynchronously.
        /// </summary>
        /// <param name="memory">The memory.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable <see cref="ValueTask"/> with <see cref="int"/> as type parameter; with the bytes count received.</returns>
        protected abstract ValueTask<int> ReceivePacketAsync(Memory<byte> memory, CancellationToken cancellationToken);

        private Task StartReadingTask(PipeReader pipeReader)
        {
            return Task.Run(async () =>
            {
                try
                {
                    await ReadPipeAsync(pipeReader, _cancellationTokenSource.Token);
                    if (!_cancellationTokenSource.IsCancellationRequested)
                    {
                        _cancellationTokenSource.Cancel();
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
            });
        }
        private Task StartWritingTask(PipeWriter pipeWriter)
        {
            return Task.Run(async () =>
            {
                try
                {
                    await FillPipeAsync(pipeWriter, _cancellationTokenSource.Token);
                    if (!_cancellationTokenSource.IsCancellationRequested)
                    {
                        _cancellationTokenSource.Cancel();
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
            });
        }
        private async Task FillPipeAsync(PipeWriter pipeWriter, CancellationToken cancellationToken)
        {
            while (true)
            {
                while (Interlocked.CompareExchange(ref _bytesReceived, 0, 0) < Interlocked.CompareExchange(ref _minimumBufferSizeHint, 0, 0))
                {
                    if (Interlocked.CompareExchange(ref _expectingHeader, 0, 0) == 1 && Interlocked.CompareExchange(ref _bytesReceived, 0, 0) > 0)
                        break;
                    if (Interlocked.CompareExchange(ref _expectingHeader, 0, 0) == 0 && Interlocked.CompareExchange(ref _bytesReceived, 0, 0) == Interlocked.CompareExchange(ref _minimumBufferSizeHint, 0, 0))
                        break;
                    try
                    {
                        if (Interlocked.CompareExchange(ref _minimumBufferSizeHint, 0, 0) - Interlocked.CompareExchange(ref _bytesReceived, 0, 0) <= 0)
                            break;
                        Memory<byte> memory = pipeWriter.GetMemory(Interlocked.CompareExchange(ref _minimumBufferSizeHint, 0, 0) - Interlocked.CompareExchange(ref _bytesReceived, 0, 0));
                        int bytesReceived = await ReceivePacketAsync(memory, cancellationToken);
                        Interlocked.Add(ref _bytesReceived, bytesReceived);
                        if (bytesReceived == 0)
                            break;
                        pipeWriter.Advance(bytesReceived);
                        Debug.WriteLine($"FillPipe bytes received: {Interlocked.CompareExchange(ref _bytesReceived, 0, 0)} out of allotted size hint {Interlocked.CompareExchange(ref _minimumBufferSizeHint, 0, 0)}, expecting header: {Interlocked.CompareExchange(ref _expectingHeader, 0, 0)}");
                    }
                    catch (Exception exception)
                    {
                        Debug.WriteLine(exception.Message);
                        break;
                    }
                }

                FlushResult flushResult = await pipeWriter.FlushAsync(cancellationToken);
                Interlocked.Exchange(ref _bytesReceived, 0);

                if (flushResult.IsCompleted)
                    break;
            }

            pipeWriter.Complete();
        }
        private async Task ReadPipeAsync(PipeReader pipeReader, CancellationToken cancellationToken)
        {
            MessageHeader? header = null;
            while (true)
            {
                ReadResult result = await pipeReader.ReadAsync(cancellationToken);
                ReadOnlySequence<byte> buffer = result.Buffer;
                SequencePosition? position = null;
                
                while (true)
                {
                    if (header is null)
                    {
                        if (!MessageHeader.TryParseHeader(buffer, out header) || header is null)
                            break;
                        position = Position(position, header.Value.GetHeaderSize(), buffer);
                        buffer = buffer.Slice(position.Value);
                        // Increase read buffer for body size
                        Interlocked.Exchange(ref _expectingHeader, 0);
                        Interlocked.Exchange(ref _minimumBufferSizeHint, header.Value.BodySize);
                        Debug.WriteLine($"MessageHeader: {header.Value.TypeName}, BodySize: {Interlocked.CompareExchange(ref _minimumBufferSizeHint, 0, 0)}");
                    }

                    byte[] bodyBufferSource = ArrayPool<byte>.Shared.Rent(header.Value.BodySize);
                    Memory<byte> bodyBufferMemory = bodyBufferSource.AsMemory(0, header.Value.BodySize);
                    if (!TryCopyBodyBuffer(buffer, header.Value.BodySize, bodyBufferMemory.Span))
                    {
                        ArrayPool<byte>.Shared.Return(bodyBufferSource);
                        break;
                    }

                    position = Position(position, header.Value.BodySize, buffer);
                    buffer = buffer.Slice(position.Value);
                    Debug.WriteLine("Body read!");

                    // Reset read buffer to minimum buffer size
                    Interlocked.Exchange(ref _expectingHeader, 1);
                    Interlocked.Exchange(ref _minimumBufferSizeHint, AbsoluteMinimumBufferSizeHint);

                    _ = ProcessMessageTask(cancellationToken, header.Value.TypeName, bodyBufferMemory, bodyBufferSource);
                    // Clear header
                    header = null;
                }

                pipeReader.AdvanceTo(buffer.Start, buffer.End);
                // Stop reading if there's no more data coming
                if (result.IsCompleted)
                    break;
            }

            // Mark the PipeReader as complete
            pipeReader.Complete();
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
        private static bool TryReadBody(int bodySize, out byte[] bodyBufferSource, out Memory<byte> bodyBufferMemory, ref ReadOnlySequence<byte> buffer, ref SequencePosition? position)
        {
            bodyBufferSource = ArrayPool<byte>.Shared.Rent(bodySize);
            bodyBufferMemory = bodyBufferSource.AsMemory(0, bodySize);
            if (!TryCopyBodyBuffer(buffer, bodySize, bodyBufferMemory.Span))
            {
                ArrayPool<byte>.Shared.Return(bodyBufferSource);
                return true;
            }
            position = Position(position, bodySize, buffer);
            buffer = buffer.Slice(position.Value);
            return false;
        }
        private static bool TryReadHeader(ref MessageHeader? header, ref ReadOnlySequence<byte> buffer, ref SequencePosition? position)
        {
            if (!(header is null))
                return false;
            if (!MessageHeader.TryParseHeader(buffer, out header) || header is null)
                return true;
            position = Position(position, header.Value.GetHeaderSize(), buffer);
            buffer = buffer.Slice(position.Value);
            return false;
        }
        private static SequencePosition Position(SequencePosition? position, long offset, ReadOnlySequence<byte> buffer)
        {
            return position is null
                ? buffer.GetPosition(offset)
                : buffer.GetPosition(offset, position.Value);
        }
        private static bool TryCopyBodyBuffer(in ReadOnlySequence<byte> buffer, int bodySize, Span<byte> destination)
        {
            if (buffer.Length < bodySize)
                return false;
            buffer.Slice(buffer.Start, bodySize).CopyTo(destination);
            return true;
        }
    }
}
