using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Neuralm.Services.Common.Application.Interfaces;

namespace Neuralm.Services.Common.Infrastructure
{
    /// <summary>
    /// Represents the <see cref="MessageTypeCache"/> class.
    /// </summary>
    public class MessageTypeCache : IMessageTypeCache
    {
        private readonly ConcurrentDictionary<string, Type> _typeCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageTypeCache"/> class.
        /// </summary>
        public MessageTypeCache(ConcurrentDictionary<string, Type> typeCache)
        {
            _typeCache = typeCache;
        }
        
        /// <inheritdoc cref="IMessageTypeCache.TryGetMessageType(string, out Type)"/>
        public bool TryGetMessageType(string typeName, out Type type)
        {
            return _typeCache.TryGetValue(typeName, out type);
        }

        /// <inheritdoc cref="IMessageTypeCache.GetMessageTypes"/>
        public IEnumerable<Type> GetMessageTypes()
        {
            return _typeCache.Values;
        }
    }
}