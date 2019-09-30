using System;

namespace Neuralm.Services.MessageQueue.Domain
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
    }
}
