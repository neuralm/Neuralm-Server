using System;
using System.Buffers;
using System.Text;

namespace Neuralm.Services.Common.Messaging
{
    /// <summary>
    /// Represents the <see cref="Message"/> struct.
    /// </summary>
    internal readonly struct Message
    {
        /// <summary>
        /// Gets the header bytes.
        /// </summary>
        internal ReadOnlyMemory<byte> Header { get; }

        /// <summary>
        /// Gets the body bytes.
        /// </summary>
        internal ReadOnlyMemory<byte> Body { get; }


        /// <summary>
        /// Initializes an instance of the <see cref="Message"/> struct.
        /// </summary>
        /// <param name="header">The header bytes.</param>
        /// <param name="body">The body bytes.</param>
        internal Message(ReadOnlyMemory<byte> header, ReadOnlyMemory<byte> body)
        {
            Body = body;
            Header = header;
        }
    }

    /// <summary>
    /// Represents the <see cref="MessageHeader"/> struct.
    /// </summary>
    internal struct MessageHeader
    {
        /// <summary>
        /// Gets the body size.
        /// </summary>
        internal int BodySize { get; }

        /// <summary>
        /// Gets the message type name.
        /// </summary>
        internal string TypeName { get; private set; }

        /// <summary>
        /// Initializes an instance of the <see cref="MessageHeader"/> struct.
        /// </summary>
        /// <param name="messageBodySize">The message body size.</param>
        private MessageHeader(int messageBodySize)
        {
            BodySize = messageBodySize;
            TypeName = string.Empty;
        }

        /// <summary>
        /// Calculates and gets the header size.
        /// </summary>
        /// <returns>Returns the header size.</returns>
        internal long GetHeaderSize()
        {
            byte[] bodySizeBytes = BitConverter.GetBytes(BodySize);
            byte[] typeNameBytes = Encoding.UTF8.GetBytes(TypeName);
            return 4 + bodySizeBytes.Length + typeNameBytes.Length;
        }

        /// <summary>
        /// Parses the buffer into a <see cref="MessageHeader"/> struct.
        /// </summary>
        /// <param name="buffer">The buffer bytes.</param>
        /// <returns>Returns a <see cref="MessageHeader"/> struct.</returns>
        private static MessageHeader ParseHeader(byte[] buffer)
        {
            int headerSize = BitConverter.ToInt32(buffer, 0);
            int bodySize = BitConverter.ToInt32(buffer, 4);
            MessageHeader header = new MessageHeader(bodySize)
            {
                TypeName = Encoding.UTF8.GetString(buffer, 8, headerSize - 8)
            };
            return header;
        }

        /// <summary>
        /// Tries to parse a sequence of bytes into a <see cref="MessageHeader"/> struct.
        /// </summary>
        /// <param name="sequence">The bytes.</param>
        /// <param name="messageHeader">The message header.</param>
        /// <returns>Returns <c>true</c> If the sequence of bytes is successfully parsed into a <see cref="MessageHeader"/> struct; otherwise, <c>false</c>.</returns>
        internal static bool TryParseHeader(ReadOnlySequence<byte> sequence, out MessageHeader? messageHeader)
        {
            if (!TryParseHeaderSize(sequence, out int headerSize) || headerSize == 0 || sequence.Length < headerSize)
            {
                messageHeader = null;
                return false;
            }

            try
            {
                byte[] buffer = ArrayPool<byte>.Shared.Rent(headerSize);
                sequence.Slice(sequence.Start, headerSize).CopyTo(buffer);
                messageHeader = ParseHeader(buffer);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                messageHeader = null;
                return false;
            }
            
            return true;
        }

        /// <summary>
        /// Tries to parse a buffer into a header size.
        /// </summary>
        /// <param name="buffer">The bytes.</param>
        /// <param name="headerSize">The header size.</param>
        /// <returns>Returns <c>true</c> If the buffer is successfully parsed into an <see cref="int"/>; otherwise, <c>false</c>.</returns>
        private static bool TryParseHeaderSize(in ReadOnlySequence<byte> buffer, out int headerSize)
        {
            if (buffer.Length < 4)
            {
                headerSize = -1;
                return false;
            }

            try
            {
                byte[] headerSizeBuffer = ArrayPool<byte>.Shared.Rent(4);
                buffer.Slice(buffer.Start, 4).CopyTo(headerSizeBuffer);
                headerSize = BitConverter.ToInt32(headerSizeBuffer, 0);
                ArrayPool<byte>.Shared.Return(headerSizeBuffer);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                headerSize = -1;
                return false;
            }
            
            return true;
        }
    }
}
