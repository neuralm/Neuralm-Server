using Neuralm.Services.Common.Messages.Interfaces;
using System;

namespace Neuralm.Services.Common.Messages.Abstractions
{
    /// <summary>
    /// Represents the <see cref="Event"/> class.
    /// </summary>
    public abstract class Event : IMessage
    {
        /// <summary>
        /// Gets the id.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Gets the date time.
        /// </summary>
        public DateTime DateTime { get; }

        /// <summary>
        /// Initializes an instance of the <see cref="Event"/> class.
        /// </summary>
        protected Event()
        {
            Id = Guid.NewGuid();
            DateTime = DateTime.UtcNow;
        }
    }
}
