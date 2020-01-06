using System;
using System.Runtime.Serialization;

namespace Neuralm.Services.MessageQueue.NeuralmMQ
{
    /// <summary>
    /// Represents the <see cref="EmptyCertificateCollectionException"/> class.
    /// Thrown when the certificate collection is empty.
    /// </summary>
    [Serializable]
    public class EmptyCertificateCollectionException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyCertificateCollectionException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public EmptyCertificateCollectionException(string message) : base(message)
        {
        }

        protected EmptyCertificateCollectionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}