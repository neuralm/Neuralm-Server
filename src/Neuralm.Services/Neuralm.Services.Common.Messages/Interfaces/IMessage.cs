using System;

namespace Neuralm.Services.Common.Messages.Interfaces
{
    /// <summary>
    /// Represents the <see cref="IMessage"/> interface.
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// Gets the id.
        /// </summary>
        Guid Id { get; set; }
    }
}
