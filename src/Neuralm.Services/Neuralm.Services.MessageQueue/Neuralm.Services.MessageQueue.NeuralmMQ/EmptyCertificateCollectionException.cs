using System;

namespace Neuralm.Services.MessageQueue.NeuralmMQ
{
    /// <summary>
    /// Represents the <see cref="EmptyCertificateCollectionException"/> class.
    /// Thrown when the certificate collection is empty.
    /// </summary>
    [Serializable]
    internal class EmptyCertificateCollectionException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyCertificateCollectionException"/> class.
        /// </summary>
        /// <param name="message"></param>
        public EmptyCertificateCollectionException(string message) : base(message)
        {
        }
    }
}