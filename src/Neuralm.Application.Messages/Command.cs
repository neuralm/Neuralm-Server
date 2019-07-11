using System;

namespace Neuralm.Application.Messages
{
    public abstract class Command : IEvent
    {
        public Guid Id { get; }
        public DateTime DateTime { get; }

        protected Command()
        {
            Id = Guid.NewGuid();
            DateTime = DateTime.UtcNow;
        }
    }
}
