using System;

namespace Neuralm.Application.Messages
{
    public interface ICommand
    {
        Guid Id { get; }
    }
}
