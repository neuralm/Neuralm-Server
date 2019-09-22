using System;

namespace Neuralm.Services.Common.Messaging.Messages
{
    /// <summary>
    /// Represents the <see cref="ICommand"/> interface.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Gets the id.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Gets the date time.
        /// </summary>
        DateTime DateTime { get; }
    }
}
