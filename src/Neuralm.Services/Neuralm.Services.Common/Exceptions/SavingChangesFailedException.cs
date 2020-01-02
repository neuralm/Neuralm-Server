using System;
using System.Runtime.Serialization;

namespace Neuralm.Services.Common.Exceptions
{
    /// <summary>
    /// Represents the <see cref="SavingChangesFailedException"/> used to throw an Exception when saving changes goes wrong.
    /// </summary>
    [Serializable]
    public class SavingChangesFailedException : Exception
    {
        public SavingChangesFailedException()
        {
        }

        public SavingChangesFailedException(string message) : base(message)
        {
        }

        public SavingChangesFailedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SavingChangesFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}