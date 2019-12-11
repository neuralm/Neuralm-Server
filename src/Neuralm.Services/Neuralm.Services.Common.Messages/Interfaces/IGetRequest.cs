using System;

namespace Neuralm.Services.Common.Messages.Interfaces
{
    /// <summary>
    /// Represents the <see cref="IGetRequest"/> interface.
    /// </summary>
    public interface IGetRequest
    {
        /// <summary>
        /// Gets the id for the get request.
        /// </summary>
        Guid GetId { get; set; } 
    }
}