using System;
using System.Runtime.Serialization;

namespace Neuralm.Services.Common.Exceptions
{
    /// <summary>
    /// Represents the <see cref="CreatingEntityFailedException"/> class used to throw an Exception when creating an Entity goes wrong.
    /// </summary>
    [Serializable]
    public class CreatingEntityFailedException : Exception
    {
        public CreatingEntityFailedException()
        {
        }

        public CreatingEntityFailedException(string message) : base(message)
        {
        }

        public CreatingEntityFailedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CreatingEntityFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
