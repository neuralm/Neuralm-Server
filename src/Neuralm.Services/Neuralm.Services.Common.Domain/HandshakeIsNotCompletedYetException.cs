using System;
using System.Runtime.Serialization;

namespace Neuralm.Services.Common.Domain
{
    /// <summary>
    /// Represents the <see cref="HandshakeIsNotCompletedYetException"/> class.
    /// Thrown when the websocket connecter has not yet completed its handshake.
    /// </summary>
    [Serializable]
    public class HandshakeIsNotCompletedYetException : Exception, ISerializable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HandshakeIsNotCompletedYetException"/> class.
        /// </summary>
        /// <param name="message"></param>
        public HandshakeIsNotCompletedYetException(string message) : base(message)
        {
        }

        protected HandshakeIsNotCompletedYetException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
