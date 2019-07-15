using System;

namespace Neuralm.Application.Messages.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public DateTime TimestampCreated { get; set; }
    }
}
