using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("Neuralm.Infrastructure", AllInternalsVisible = true)]
namespace Neuralm.Application.Messages
{
    internal readonly struct Message
    {
        internal ReadOnlyMemory<byte> Header { get; }
        internal ReadOnlyMemory<byte> Body { get; }

        internal Message(ReadOnlyMemory<byte> header, ReadOnlyMemory<byte> body)
        {
            Body = body;
            Header = header;
        }
    }

    internal struct MessageHeader
    {
        internal int BodySize { get; }
        internal string TypeName { get; private set; }

        internal MessageHeader(int messageBodySize, string typeName)
        {
            BodySize = messageBodySize;
            TypeName = typeName;
        }
        private MessageHeader(int messageBodySize)
        {
            BodySize = messageBodySize;
            TypeName = string.Empty;
        }

        internal long GetHeaderSize()
        {
            byte[] bodySizeBytes = BitConverter.GetBytes(BodySize);
            byte[] typeNameBytes = Encoding.Default.GetBytes(TypeName);
            return 4 + bodySizeBytes.Length + typeNameBytes.Length;
        }

        internal static MessageHeader ParseHeader(byte[] buffer)
        {
            int headerSize = BitConverter.ToInt32(buffer, 0);
            int bodySize = BitConverter.ToInt32(buffer, 4);
            return new MessageHeader(bodySize)
            {
                TypeName = Encoding.Default.GetString(buffer, 8, headerSize - 8)
            };
        }
        internal static bool TryParseHeader(ReadOnlySequence<byte> sequence, out MessageHeader? header)
        {
            if (!TryParseHeaderSize(sequence, out int headerSize))
            {
                header = null;
                return false;
            }

            if (sequence.Length < headerSize)
            {
                header = null;
                return false;
            }

            byte[] buffer = ArrayPool<byte>.Shared.Rent(headerSize);
            sequence.Slice(sequence.Start, headerSize).CopyTo(buffer);
            header = ParseHeader(buffer);
            return true;
        }
        internal static bool TryParseHeaderSize(in ReadOnlySequence<byte> buffer, out int headerSize)
        {
            if (buffer.Length < 4)
            {
                headerSize = -1;
                return false;
            }

            byte[] headerSizeBuffer = ArrayPool<byte>.Shared.Rent(4);
            buffer.Slice(buffer.Start, 4).CopyTo(headerSizeBuffer);
            headerSize = BitConverter.ToInt32(headerSizeBuffer, 0);
            ArrayPool<byte>.Shared.Return(headerSizeBuffer);
            return true;
        }
    }
}
