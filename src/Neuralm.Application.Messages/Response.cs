using System;

namespace Neuralm.Application.Messages
{
    public abstract class Response : IResponse
    {
        public Guid Id { get; }
        public Guid RequestId { get; }
        public DateTime DateTime { get; }
        public bool Success { get; }

        protected Response(Guid requestId, bool success)
        {
            Id = Guid.NewGuid();
            RequestId = requestId;
            DateTime = DateTime.UtcNow;
            Success = success;
        }
    }
}
