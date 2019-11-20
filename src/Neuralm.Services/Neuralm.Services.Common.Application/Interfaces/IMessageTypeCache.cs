using System;
using System.Collections.Generic;

namespace Neuralm.Services.Common.Application.Interfaces
{
    /// <summary>
    /// Represents the <see cref="IMessageTypeCache"/> interface.
    /// </summary>
    public interface IMessageTypeCache
    {
        /// <summary>
        /// Tries to get a message type with the given type name.
        /// </summary>
        /// <param name="typeName">The type name.</param>
        /// <param name="type">The type.</param>
        /// <returns>Returns <c>true</c> if the type name is found; otherwise, <c>false</c>.</returns>
        bool TryGetMessageType(string typeName, out Type type);

        /// <summary>
        /// Gets all message types.
        /// </summary>
        /// <returns>Returns all message types.</returns>
        IEnumerable<Type> GetMessageTypes();
    }
}