using System;
using System.Runtime.Serialization;

namespace Neuralm.Presentation.CLI
{
    /// <summary>
    /// The <see cref="EmptyCertificateCollectionException"/> class used to throw an exception when the certificate collection is empty.
    /// </summary>
    internal class EmptyCertificateCollectionException : Exception
    {
        public EmptyCertificateCollectionException()
        {
        }

        protected EmptyCertificateCollectionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public EmptyCertificateCollectionException(string message) : base(message)
        {
        }

        public EmptyCertificateCollectionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}