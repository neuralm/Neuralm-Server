﻿using Neuralm.Services.MessageQueue.Application.Interfaces;
using System;
using System.Text;
using System.Text.Json;

namespace Neuralm.Services.MessageQueue.Application.Serializers
{
    /// <summary>
    /// Represents the <see cref="JsonMessageSerializer"/> class an implementation of the <see cref="IMessageSerializer"/> interface.
    /// </summary>
    public sealed class JsonMessageSerializer : IMessageSerializer
    {
        /// <inheritdoc cref="IMessageSerializer.Serialize"/>
        public Memory<byte> Serialize(object message)
        {
            return new Memory<byte>(JsonSerializer.SerializeToUtf8Bytes(message));
        }

        /// <inheritdoc cref="IMessageSerializer.Deserialize{T}"/>
        public T Deserialize<T>(Memory<byte> message)
        {
            return JsonSerializer.Deserialize<T>(message.Span);
        }

        /// <inheritdoc cref="IMessageSerializer.Deserialize"/>
        public object Deserialize(Memory<byte> message, Type type)
        {
            return JsonSerializer.Deserialize(message.Span, type);
        }
    }
}
