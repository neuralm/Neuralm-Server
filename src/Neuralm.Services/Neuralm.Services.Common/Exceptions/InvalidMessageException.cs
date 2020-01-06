using System;
using System.Runtime.Serialization;

namespace Neuralm.Services.Common.Exceptions
{
    /// <summary>
    /// Represents the <see cref="InvalidMessageException"/> used to throw an Exception when an invalid message is encountered.
    /// </summary>
    [Serializable]
    public class InvalidMessageException : Exception
    {
        public InvalidMessageException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidMessageException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public InvalidMessageException(string message) : base(message)
        {
        }

        public InvalidMessageException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidMessageException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
