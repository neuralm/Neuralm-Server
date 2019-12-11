using Neuralm.Services.Common.Messages.Interfaces;
using System;

namespace Neuralm.Services.Common.Messages.Abstractions
{
    /// <summary>
    /// Represents the <see cref="Response"/> class.
    /// </summary>
    public abstract class Response : IResponseMessage
    {
        /// <summary>
        /// Gets the id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets the request id.
        /// </summary>
        public Guid RequestId { get; set; }

        /// <summary>
        /// Gets the date time.
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Gets the message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets a value indicating whether the response was successful.
        /// </summary>
        public bool Success { get; set; }

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

        /// <summary>
        /// Initializes a new instance of the <see cref="Response"/> class.
        /// SERIALIZATION CONSTRUCTOR.
        /// </summary>
        protected Response()
        {
            Id = Guid.NewGuid();
            DateTime = DateTime.UtcNow;
        }
    }
}
