using System;
using System.Runtime.Serialization;

namespace Neuralm.Domain.Exceptions
{
    /// <summary>
    /// Represents the <see cref="UpdatingEntityFailedException"/> used to throw an Exception when updating an Entity goes wrong.
    /// </summary>
    [Serializable]
    public class UpdatingEntityFailedException : Exception
    {
        public UpdatingEntityFailedException()
        {
        }

        public UpdatingEntityFailedException(string message) : base(message)
        {
        }

        public UpdatingEntityFailedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UpdatingEntityFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
