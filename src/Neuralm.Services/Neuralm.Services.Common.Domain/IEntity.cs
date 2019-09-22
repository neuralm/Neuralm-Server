using System;

namespace Neuralm.Services.Common.Domain
{
    /// <summary>
    /// Represents the <see cref="IEntity"/> interface.
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Gets and sets the id.
        /// </summary>
        Guid Id { get; }
    }
}
