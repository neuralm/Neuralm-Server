﻿using System;

namespace Neuralm.Infrastructure.Interfaces
{
    /// <summary>
    /// Represents the <see cref="IMessageSerializer"/> interface.
    /// </summary>
    public interface IMessageSerializer
    {
        /// <summary>
        /// Serializes the object into a byte array.
        /// </summary>
        /// <param name="message">The message object.</param>
        /// <returns>Return the serialized message object as byte array.</returns>
        Memory<byte> Serialize(object message);

        /// <summary>
        /// Deserializes message as the byte array into type of <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T">The message type.</typeparam>
        /// <param name="message">The message as byte array.</param>
        /// <returns>Returns a deserialized message as <see cref="T"/>.</returns>
        T Deserialize<T>(Memory<byte> message);

        /// <summary>
        /// Deserializes message as byte array into type of <see cref="type"/>.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type"></param>
        /// <returns>Returns a deserialized message as <see cref="object"/>.</returns>
        object Deserialize(Memory<byte> message, Type type);
    }
}
