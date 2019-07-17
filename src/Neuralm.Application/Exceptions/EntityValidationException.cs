using System;
using System.Runtime.Serialization;

namespace Neuralm.Application.Exceptions
{
    /// <summary>
    /// The EntityValidationException class is used to denote an exception that occured during the validation of an entity.
    /// </summary>
    [Serializable]
    internal class EntityValidationException : Exception
    {
        internal EntityValidationException()
        {
        }

        internal EntityValidationException(string message) : base(message)
        {
        }

        internal EntityValidationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        internal EntityValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
