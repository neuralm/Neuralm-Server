using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using Neuralm.Application.Messages;

namespace Neuralm.Infrastructure
{
    /// <summary>
    /// Represents the <see cref="MessageTypeCache"/> class.
    /// </summary>
    internal static class MessageTypeCache
    {
        private static readonly ConcurrentDictionary<string, Type> TypeCache = new ConcurrentDictionary<string, Type>();
        private static int _isLoaded = 0;
        private static int _isLoading = 0;

        /// <summary>
        /// Tries to get a message type with the given type name.
        /// </summary>
        /// <param name="typeName">The type name.</param>
        /// <param name="type">The type.</param>
        /// <returns>Returns <c>true</c> if the type name is found; otherwise, <c>false</c>.</returns>
        public static bool TryGetMessageType(string typeName, out Type type)
        {
            return TypeCache.TryGetValue(typeName, out type);
        }

        /// <summary>
        /// Loads all the messages in the <see cref="TypeCache"/> dictionary thread safe.
        /// </summary>
        public static void LoadMessageTypeCache()
        {
            if (Interlocked.Exchange(ref _isLoaded, 1) == 1)
                return;
            if (Interlocked.Exchange(ref _isLoading, 1) == 1)
                return;
            Interlocked.Increment(ref _isLoading);
            foreach (Type messageType in typeof(Message).Assembly.GetTypes().Where(t => t.BaseType == typeof(Request) || t.BaseType == typeof(Response) || t.BaseType == typeof(Event) || t.BaseType == typeof(Command)))
            {
                TypeCache.TryAdd(messageType.Name, messageType);
            }
            Interlocked.Decrement(ref _isLoading);
            Interlocked.Increment(ref _isLoaded);
        }
    }
}
