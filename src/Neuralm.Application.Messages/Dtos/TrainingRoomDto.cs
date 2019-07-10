using System;
using Neuralm.Domain.Entities.NEAT;

namespace Neuralm.Application.Messages.Dtos
{
    public class TrainingRoomDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public UserDto Owner { get; set; }
        public uint Generation { get; set; }
        public TrainingRoomSettings TrainingRoomSettings { get; set; }
        public double HighestScore { get; set; }
        public double LowestScore { get; set; }
        public double AverageScore { get; set; }
    }
}
