using System;

namespace Neuralm.Application.Messages
{
    public abstract class Request : IRequest
    {
        public Guid Id { get; }
        public DateTime DateTime { get; }

        protected Request()
        {
            Id = Guid.NewGuid();
            DateTime = DateTime.UtcNow;
        }
    }
}
