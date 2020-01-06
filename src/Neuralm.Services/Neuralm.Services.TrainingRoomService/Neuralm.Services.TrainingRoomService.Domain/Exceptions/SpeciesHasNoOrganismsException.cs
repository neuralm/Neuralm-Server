using System;
using System.Runtime.Serialization;

namespace Neuralm.Services.TrainingRoomService.Domain.Exceptions
{
    /// <summary>
    /// Represents the <see cref="SpeciesHasNoOrganismsException"/> class.
    /// Thrown when the species has not organisms left after a new generation.
    /// </summary>
    [Serializable]
    public class SpeciesHasNoOrganismsException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpeciesHasNoOrganismsException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public SpeciesHasNoOrganismsException(string message) : base(message)
        {
        }

        protected SpeciesHasNoOrganismsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
