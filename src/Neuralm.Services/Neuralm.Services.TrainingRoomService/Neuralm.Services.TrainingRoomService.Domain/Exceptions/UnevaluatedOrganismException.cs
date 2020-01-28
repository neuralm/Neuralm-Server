using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Neuralm.Services.TrainingRoomService.Domain.Exceptions
{
    /// <summary>
    /// Represents the <see cref="UnevaluatedOrganismException"/> class.
    /// Thrown when an organisms requires to be have been evaluated.
    /// </summary>
    [Serializable]
    public class UnevaluatedOrganismException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnevaluatedOrganismException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public UnevaluatedOrganismException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnevaluatedOrganismException"/> class.
        /// </summary>
        /// <param name="organism">The unevaluated organism.</param>
        public UnevaluatedOrganismException(Organism organism) : base($"Use of unevaluated organism: {organism}.\n Evaluate the organism before calling this method.")
        {
        }

        protected UnevaluatedOrganismException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
