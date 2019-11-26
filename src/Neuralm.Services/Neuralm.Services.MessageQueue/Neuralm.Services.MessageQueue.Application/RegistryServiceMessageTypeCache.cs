using System;
using System.Collections.Generic;
using Neuralm.Services.Common.Application.Interfaces;
using Neuralm.Services.MessageQueue.Application.Interfaces;

namespace Neuralm.Services.MessageQueue.Application
{
    /// <summary>
    /// Represents the <see cref="RegistryServiceMessageTypeCache"/> class.
    /// </summary>
    public class RegistryServiceMessageTypeCache : IRegistryServiceMessageTypeCache
    {
        private readonly IMessageTypeCache _messageTypeCache;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryServiceMessageTypeCache"/> class.
        /// </summary>
        /// <param name="messageTypeCache">The message type cache.</param>
        public RegistryServiceMessageTypeCache(IMessageTypeCache messageTypeCache)
        {
            _messageTypeCache =  messageTypeCache;
        }

        /// <inheritdoc cref="IMessageTypeCache.TryGetMessageType(string, out Type)"/>
        public bool TryGetMessageType(string typeName, out Type type)
        {
            return _messageTypeCache.TryGetMessageType(typeName, out type);
        }
        
        /// <inheritdoc cref="IMessageTypeCache.GetMessageTypes()"/>
        public IEnumerable<Type> GetMessageTypes()
        {
            return _messageTypeCache.GetMessageTypes();
        }
    }
}