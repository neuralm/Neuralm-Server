using System;

namespace Neuralm.Application.Messages
{
    /// <summary>
    /// Represents the <see cref="IResponse"/> interface.
    /// </summary>
    public interface IResponse
    {
        /// <summary>
        /// Gets the id.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Gets the request id.
        /// </summary>
        Guid RequestId { get; }

        /// <summary>
        /// Gets the date time.
        /// </summary>
        DateTime DateTime { get; }

        /// <summary>
        /// Gets the value indicating whether the response is successful.
        /// </summary>
        bool Success { get; }
    }
}