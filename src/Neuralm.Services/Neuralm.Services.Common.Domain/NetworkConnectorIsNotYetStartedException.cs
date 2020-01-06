using System;
using System.Runtime.Serialization;

namespace Neuralm.Services.Common.Domain
{
    /// <summary>
    /// Represents the <see cref="NetworkConnectorIsNotYetStartedException"/> class.
    /// Thrown when the network connecter is not yet started.
    /// </summary>
    [Serializable]
    public class NetworkConnectorIsNotYetStartedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkConnectorIsNotYetStartedException"/> class.
        /// </summary>
        /// <param name="message"></param>
        public NetworkConnectorIsNotYetStartedException(string message) : base(message)
        {
        }

        protected NetworkConnectorIsNotYetStartedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
