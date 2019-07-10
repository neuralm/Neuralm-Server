using System;

namespace Neuralm.Application.Messages.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime TimestampCreated { get; set; }
    }
}
