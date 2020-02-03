using Neuralm.Services.Common.Application.Interfaces;
using Neuralm.Services.Common.Messages.Interfaces;
using Neuralm.Services.Common.Patterns;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Neuralm.Services.Common.Infrastructure
{
    /// <summary>
    /// Represents the <see cref="MessageTypeCacheFactory"/> class.
    /// </summary>
    public class MessageTypeCacheFactory : IFactory<IMessageTypeCache, IEnumerable<Type>>
    {
        /// <inheritdoc cref="IFactory{IMessageTypeCache, IEnumerable}.Create(IEnumerable)"/>
        public IMessageTypeCache Create(IEnumerable<Type> parameter)
        {
            ConcurrentDictionary<string, Type> typeCache = new ConcurrentDictionary<string, Type>();
            parameter
                .SelectMany(GetMessageTypes)
                .ToList()
                .ForEach(messageType => typeCache.TryAdd(messageType.FullName, messageType));
            
            static IEnumerable<Type> GetMessageTypes(Type type) => 
                Assembly.GetAssembly(type).GetTypes()
                    .Where(type2 => typeof(IMessage).IsAssignableFrom(type2));
         
            return new MessageTypeCache(typeCache);
        }
    }
}