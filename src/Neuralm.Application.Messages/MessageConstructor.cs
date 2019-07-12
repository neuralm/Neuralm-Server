using System;
using System.Linq;
using System.Text;
using Neuralm.Infrastructure.Interfaces;

namespace Neuralm.Application.Messages
{
    internal class MessageConstructor
    {
        private readonly IMessageSerializer _messageSerializer;

        internal MessageConstructor(IMessageSerializer messageSerializer)
        {
            _messageSerializer = messageSerializer ?? throw new ArgumentNullException(nameof(messageSerializer));
        }

        internal Message ConstructMessage(object messageBody)
        {
            ReadOnlyMemory<byte> body = _messageSerializer.Serialize(messageBody);
            byte[] bodySizeBytes = BitConverter.GetBytes(body.Length);
            byte[] typeNameBytes = Encoding.UTF8.GetBytes(messageBody.GetType().Name);
            byte[] headerSizeBytes = BitConverter.GetBytes(4 + bodySizeBytes.Length + typeNameBytes.Length);
            ReadOnlyMemory<byte> header = headerSizeBytes.Concat(bodySizeBytes).Concat(typeNameBytes).ToArray();
            return new Message(header, body);
        }

        internal object DeconstructMessageBody(Memory<byte> messageBody, Type type)
        {
            return _messageSerializer.Deserialize(messageBody, type);
        }
    }
}
