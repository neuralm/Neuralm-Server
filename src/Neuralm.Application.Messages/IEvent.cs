using System;

namespace Neuralm.Application.Messages
{
    public interface IEvent
    {
        Guid Id { get; }
        DateTime DateTime { get; }
    }
}
