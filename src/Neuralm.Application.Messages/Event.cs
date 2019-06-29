using System;

namespace Neuralm.Application.Messages
{
    public abstract class Event : IEvent
    {
        public Guid Id { get; }
        public DateTime DateTime { get; }

        protected Event()
        {
            Id = Guid.NewGuid();
            DateTime = DateTime.UtcNow;
        }
    }
}
