using System;
using System.Runtime.Serialization;

namespace Neuralm.Application.Exceptions
{
    /// <summary>
    /// The NetworkConnectorIsNotYetConnectedException class; used to throw an Exception when the connector is not yet connected.
    /// </summary>
    [Serializable]
    public class NetworkConnectorIsNotYetConnectedException : Exception
    {
        public NetworkConnectorIsNotYetConnectedException()
        {
        }

        public NetworkConnectorIsNotYetConnectedException(string message) : base(message)
        {
        }

        public NetworkConnectorIsNotYetConnectedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NetworkConnectorIsNotYetConnectedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
