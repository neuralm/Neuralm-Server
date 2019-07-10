using Neuralm.Application.Interfaces;
using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.IO.Pipelines;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Neuralm.Application.Messages;
using Neuralm.Infrastructure.Interfaces;

namespace Neuralm.Infrastructure.Networking
{
    public abstract class BaseNetworkConnector : INetworkConnector
    {
        private readonly IMessageProcessor _messageProcessor;
        private readonly MessageConstructor _messageConstructor;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private static readonly ConcurrentDictionary<string, Type> TypeCache = new ConcurrentDictionary<string, Type>();
        private const int MinimumBufferSize = 512;

        public bool IsRunning { get; private set; }
        public abstract bool IsConnected { get; }

        protected BaseNetworkConnector(IMessageSerializer messageSerializer, IMessageProcessor messageProcessor)
        {
            _messageConstructor = new MessageConstructor(messageSerializer);
            _messageProcessor = messageProcessor ?? throw new ArgumentNullException(nameof(messageProcessor));
        }

        public void Start()
        {
            if (!IsConnected)
                throw new Exception("NetworkConnector has not connected yet. Call ConnectAsync() first.");

            Pipe pipe = new Pipe();

            _ = StartWritingTask(pipe.Writer);
            _ = StartReadingTask(pipe.Reader);

            IsRunning = true;
        }
        public void Stop()
        {
            _cancellationTokenSource.Cancel();
            IsRunning = false;
        }
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
        public async Task SendMessageAsync<TMessage>(TMessage message, CancellationToken cancellationToken)
        {
            if (!IsRunning)
                throw new Exception("NetworkConnector has not started yet. Call ConnectAsync() first");

            Message constructedMessage = _messageConstructor.ConstructMessage(message);
            await SendPacketAsync(constructedMessage.Header, cancellationToken);
            await SendPacketAsync(constructedMessage.Body, cancellationToken);
        }
        private async Task FillPipeAsync(PipeWriter pipeWriter, CancellationToken cancellationToken)
        {
            while (true)
            {
                // Allocate at least 512 bytes from the PipeWriter
                Memory<byte> memory = pipeWriter.GetMemory(MinimumBufferSize);
                try
                {
                    int bytesReceived = await ReceivePacketAsync(memory, cancellationToken);
                    if (bytesReceived == 0)
                        break;
                    // Tell the PipeWriter how much was read from the Socket
                    pipeWriter.Advance(bytesReceived);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    break;
                }

                // Make the data available to the PipeReader
                FlushResult flushResult = await pipeWriter.FlushAsync(cancellationToken);

                if (flushResult.IsCompleted)
                    break;
            }
            // Tell the PipeReader that there's no more data coming
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
                    if (TryReadHeader(ref header, ref buffer, ref position))
                        break;
                    if (TryReadBody(header.Value.BodySize, out byte[] bodyBufferSource, out Memory<byte> bodyBufferMemory, ref buffer, ref position))
                        break;
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
                if (GetType(typeName, out Type type))
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
        private static bool GetType(string typeName, out Type type)
        {
            if (TypeCache.TryGetValue(typeName, out type))
            {
                return true;
            }
            type = Type.GetType(typeName);
            if (string.IsNullOrWhiteSpace(typeName))
            {
                return false;
            }
            if (!(type is null))
            {
                TypeCache.TryAdd(typeName, type);
                return true;
            }

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = assembly.GetType(typeName);

                if (type is null) continue;
                TypeCache.TryAdd(typeName, type);
                return true;
            }

            return false;
        }
        private static bool TryCopyBodyBuffer(in ReadOnlySequence<byte> buffer, int bodySize, Span<byte> destination)
        {
            if (buffer.Length < bodySize)
                return false;
            buffer.Slice(buffer.Start, bodySize).CopyTo(destination);
            return true;
        }
        public abstract ValueTask ConnectAsync(CancellationToken cancellationToken);
        protected abstract Task<int> SendPacketAsync(ReadOnlyMemory<byte> packet, CancellationToken cancellationToken);
        protected abstract Task<int> ReceivePacketAsync(Memory<byte> memory, CancellationToken cancellationToken);
    }
}
