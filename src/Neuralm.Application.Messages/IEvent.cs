using System;

namespace Neuralm.Application.Messages
{
    /// <summary>
    /// The interface for a Event.
    /// </summary>
    public interface IEvent
    {
        Guid Id { get; }
        DateTime DateTime { get; }
    }
}
