using System;

namespace Neuralm.Application.Messages
{
    /// <summary>
    /// The interface for a Command.
    /// </summary>
    public interface ICommand
    {
        Guid Id { get; }
    }
}
