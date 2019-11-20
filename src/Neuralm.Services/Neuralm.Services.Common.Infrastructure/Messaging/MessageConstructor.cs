using Neuralm.Services.Common.Application.Interfaces;
using System;
using System.Linq;
using System.Text;

namespace Neuralm.Services.Common.Infrastructure.Messaging
{
    /// <summary>
    /// Represents the <see cref="MessageConstructor"/> class.
    /// </summary>
    internal class MessageConstructor
    {
        private readonly IMessageSerializer _messageSerializer;

        /// <summary>
        /// Initializes an instance of the <see cref="MessageConstructor"/> class.
        /// </summary>
        /// <param name="messageSerializer"></param>
        internal MessageConstructor(IMessageSerializer messageSerializer)
        {
            _messageSerializer = messageSerializer ?? throw new ArgumentNullException(nameof(messageSerializer));
        }

        /// <summary>
        /// Constructs a sendable message from an object.
        /// </summary>
        /// <param name="messageBody">The object.</param>
        /// <returns>Returns a <see cref="Message"/> containing header and body bytes.</returns>
        internal Message ConstructMessage(object messageBody)
        {
            ReadOnlyMemory<byte> body = _messageSerializer.Serialize(messageBody);
            byte[] bodySizeBytes = BitConverter.GetBytes(body.Length);
            byte[] typeNameBytes = Encoding.UTF8.GetBytes(messageBody.GetType().FullName);
            byte[] headerSizeBytes = BitConverter.GetBytes(4 + bodySizeBytes.Length + typeNameBytes.Length);
            ReadOnlyMemory<byte> header = headerSizeBytes.Concat(bodySizeBytes).Concat(typeNameBytes).ToArray();
            return new Message(header, body);
        }

        /// <summary>
        /// Deconstructs a message body as bytes with a given type.
        /// </summary>
        /// <param name="messageBody">The memory body in bytes.</param>
        /// <param name="type">The type.</param>
        /// <returns>Returns the message body as an object.</returns>
        internal object DeconstructMessageBody(Memory<byte> messageBody, Type type)
        {
            return _messageSerializer.Deserialize(messageBody, type);
        }
    }
}
