using System;
using System.Runtime.Serialization;

namespace Neuralm.Domain.Exceptions
{
    /// <summary>
    /// Represents the <see cref="EntityAlreadyExistsException"/> used to throw an Exception when a predicate returns with a duplicate exception.
    /// </summary>
    [Serializable]
    public class EntityAlreadyExistsException : Exception
    {
        public EntityAlreadyExistsException()
        {
        }

        public EntityAlreadyExistsException(string message) : base(message)
        {
        }

        public EntityAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EntityAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
