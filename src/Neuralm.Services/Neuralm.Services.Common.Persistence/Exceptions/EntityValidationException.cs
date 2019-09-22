using System;
using System.Runtime.Serialization;

namespace Neuralm.Services.Common.Persistence.Exceptions
{
    /// <summary>
    /// The <see cref="EntityValidationException"/> class is used to denote an exception that occured during the validation of an entity.
    /// </summary>
    [Serializable]
    public class EntityValidationException : Exception
    {
        public EntityValidationException()
        {
        }

        public EntityValidationException(string message) : base(message)
        {
        }

        public EntityValidationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public EntityValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
