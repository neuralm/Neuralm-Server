using System;

namespace Neuralm.Services.Common.Infrastructure.Messaging
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
}
