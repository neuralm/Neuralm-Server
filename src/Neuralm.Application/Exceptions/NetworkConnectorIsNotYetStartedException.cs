using System;
using System.Runtime.Serialization;

namespace Neuralm.Application.Exceptions
{
    /// <summary>
    /// The <see cref="NetworkConnectorIsNotYetStartedException"/> class; used to throw an Exception when the connector is not yet started.
    /// </summary>
    [Serializable]
    public class NetworkConnectorIsNotYetStartedException : Exception
    {
        public NetworkConnectorIsNotYetStartedException()
        {
        }

        public NetworkConnectorIsNotYetStartedException(string message) : base(message)
        {
        }

        public NetworkConnectorIsNotYetStartedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NetworkConnectorIsNotYetStartedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
