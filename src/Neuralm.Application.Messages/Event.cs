using System;

namespace Neuralm.Application.Messages
{
    /// <summary>
    /// Represents the <see cref="Event"/> class.
    /// </summary>
    public abstract class Event : IEvent
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
