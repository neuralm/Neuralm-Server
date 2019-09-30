using Neuralm.Services.Common.Messages.Interfaces;
using System;

namespace Neuralm.Services.Common.Messages.Abstractions
{
    /// <summary>
    /// Represents the <see cref="Response"/> class.
    /// </summary>
    public abstract class Response : IResponse
    {
        /// <summary>
        /// Gets the id.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Gets the request id.
        /// </summary>
        public Guid RequestId { get; }

        /// <summary>
        /// Gets the date time.
        /// </summary>
        public DateTime DateTime { get; }

        /// <summary>
        /// Gets the message.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Gets a value indicating whether the response was successful.
        /// </summary>
        public bool Success { get; }

        /// <summary>
        /// Initializes an instance of the <see cref="Response"/> class.
        /// </summary>
        /// <param name="requestId">The request id.</param>
        /// <param name="message">The message.</param>
        /// <param name="success">The success flag.</param>
        protected Response(Guid requestId, string message, bool success)
        {
            Id = Guid.NewGuid();
            RequestId = requestId;
            DateTime = DateTime.UtcNow;
            Message = message;
            Success = success;
        }
    }
}
