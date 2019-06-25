using System;

namespace Neuralm.Application.Messages.Requests
{
    public class CreateRoomRequest : IRequest
    {
        public Guid Id { get; }
    }
}
