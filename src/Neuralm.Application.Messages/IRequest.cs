using System;

namespace Neuralm.Application.Messages
{
    public interface IRequest
    {
        Guid Id { get; }
        DateTime DateTime { get; }
    }
}
