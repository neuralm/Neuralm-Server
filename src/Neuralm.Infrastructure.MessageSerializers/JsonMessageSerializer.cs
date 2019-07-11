using System;
using System.Text;
using Neuralm.Infrastructure.Interfaces;
using Newtonsoft.Json;

namespace Neuralm.Infrastructure.MessageSerializers
{
    public sealed class JsonMessageSerializer : IMessageSerializer
    {
        public Memory<byte> Serialize(object message)
        {
            return new Memory<byte>(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message)));
        }

        public T Deserialize<T>(Memory<byte> message)
        {
            return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(message.ToArray()));
        }

        public object Deserialize(Memory<byte> message, Type type)
        {
            return JsonConvert.DeserializeObject(Encoding.UTF8.GetString(message.ToArray()), type);
        }
    }
}
