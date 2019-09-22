using System;

namespace Neuralm.Services.Common.Messaging.Messages
{
    /// <summary>
    /// Represents the <see cref="Request"/> class.
    /// </summary>
    public abstract class Request : IRequest
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
        /// Initializes an instance of the <see cref="Request"/> class.
        /// </summary>
        protected Request()
        {
            Id = Guid.NewGuid();
            DateTime = DateTime.UtcNow;
        }
    }
}
