using System;
using System.Runtime.Serialization;

namespace Neuralm.Services.Common.Exceptions
{
    /// <summary>
    /// Represents the <see cref="InitializationException"/> class.
    /// Thrown when the initialization goes awry.
    /// </summary>
    [Serializable]
    public class InitializationException : Exception
    {
        public InitializationException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InitializationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public InitializationException(string message) : base(message)
        {
        }

        public InitializationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InitializationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}