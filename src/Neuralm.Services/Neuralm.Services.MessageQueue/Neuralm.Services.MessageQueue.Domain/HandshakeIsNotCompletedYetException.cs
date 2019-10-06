using System;

namespace Neuralm.Services.MessageQueue.Domain
{
    /// <summary>
    /// Represents the <see cref="HandshakeIsNotCompletedYetException"/> class.
    /// Thrown when the websocket connecter has not yet completed its handshake.
    /// </summary>
    [Serializable]
    public class HandshakeIsNotCompletedYetException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HandshakeIsNotCompletedYetException"/> class.
        /// </summary>
        /// <param name="message"></param>
        public HandshakeIsNotCompletedYetException(string message) : base(message)
        {
        }
    }
}
