using System;

namespace Neuralm.Services.Common.Messages.Interfaces
{
    /// <summary>
    /// Represents the <see cref="IResponseMessage"/> interface.
    /// </summary>
    public interface IResponseMessage : IMessage
    {
        /// <summary>
        /// Gets and sets the request id.
        /// </summary>
        Guid RequestId { get; set; }
        
        /// <summary>
        /// Gets and sets a value indicating whether the request was successful. 
        /// </summary>
        bool Success { get; set; }
        
        /// <summary>
        /// Gets t and sets the date time.
        /// </summary>
        DateTime DateTime { get; set; }
    }
}
