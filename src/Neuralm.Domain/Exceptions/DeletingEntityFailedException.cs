using System;
using System.Runtime.Serialization;

namespace Neuralm.Domain.Exceptions
{
    /// <summary>
    /// Represents the <see cref="DeletingEntityFailedException"/> used to throw an Exception when deleting an Entity goes wrong.
    /// </summary>
    [Serializable]
    public class DeletingEntityFailedException : Exception
    {
        public DeletingEntityFailedException()
        {
        }

        public DeletingEntityFailedException(string message) : base(message)
        {
        }

        public DeletingEntityFailedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DeletingEntityFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
