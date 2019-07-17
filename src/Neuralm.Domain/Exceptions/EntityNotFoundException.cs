using System;
using System.Runtime.Serialization;

namespace Neuralm.Domain.Exceptions
{
    /// <summary>
    /// Represents the <see cref="EntityNotFoundException"/> used to throw an Exception when the predicate returns without results.
    /// </summary>
    [Serializable]
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string message) : base(message)
        {
        }
        public EntityNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EntityNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
