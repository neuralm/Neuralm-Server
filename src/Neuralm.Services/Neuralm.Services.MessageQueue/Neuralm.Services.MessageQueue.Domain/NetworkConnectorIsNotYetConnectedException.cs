using System;

namespace Neuralm.Services.MessageQueue.Domain
{
    /// <summary>
    /// Represents the <see cref="NetworkConnectorIsNotYetConnectedException"/> class.
    /// Thrown when the network connecter is not yet connected.
    /// </summary>
    [Serializable]
    public class NetworkConnectorIsNotYetConnectedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkConnectorIsNotYetConnectedException"/> class.
        /// </summary>
        /// <param name="message"></param>
        public NetworkConnectorIsNotYetConnectedException(string message) : base(message)
        {

        }
    }
}
