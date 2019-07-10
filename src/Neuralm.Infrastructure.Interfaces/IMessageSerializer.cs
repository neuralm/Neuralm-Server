using System;

namespace Neuralm.Infrastructure.Interfaces
{
    public interface IMessageSerializer
    {
        Memory<byte> Serialize(object message);
        T Deserialize<T>(Memory<byte> message);
        object Deserialize(Memory<byte> message, Type type);
    }
}
