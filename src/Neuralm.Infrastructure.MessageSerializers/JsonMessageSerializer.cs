using System;
using System.Text;
using Neuralm.Infrastructure.Interfaces;
using Newtonsoft.Json;

namespace Neuralm.Infrastructure.MessageSerializers
{
    /// <summary>
    /// Represents the <see cref="JsonMessageSerializer"/> class an implementation of the <see cref="IMessageSerializer"/> interface.
    /// </summary>
    public sealed class JsonMessageSerializer : IMessageSerializer
    {
        /// <inheritdoc cref="IMessageSerializer.Serialize"/>
        public Memory<byte> Serialize(object message)
        {
            return new Memory<byte>(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message)));
        }

        /// <inheritdoc cref="IMessageSerializer.Deserialize{T}"/>
        public T Deserialize<T>(Memory<byte> message)
        {
            return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(message.ToArray()));
        }

        /// <inheritdoc cref="IMessageSerializer.Deserialize"/>
        public object Deserialize(Memory<byte> message, Type type)
        {
            return JsonConvert.DeserializeObject(Encoding.UTF8.GetString(message.ToArray()), type);
        }
    }
}
