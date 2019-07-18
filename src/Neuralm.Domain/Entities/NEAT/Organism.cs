using System;

namespace Neuralm.Domain.Entities.NEAT
{
    public class Organism
    {

        TrainingRoom TrainingRoom { get; }
        public Brain Brain { get; }

        /// <summary>
        /// Gets and sets the species id.
        /// </summary>
        public Guid SpeciesId { get; set; }

        /// <summary>
        /// Gets and sets the training room id.
        /// </summary>
        public Guid TrainingRoomId { get; private set; }
        public double Score { get; internal set; }

        Organism() { }

        public Organism(TrainingRoom trainingRoom)
        {
            TrainingRoomId = trainingRoom.Id;
            TrainingRoom = trainingRoom;
            Brain = new Brain(trainingRoom);
        }

        Organism(TrainingRoom trainingRoom, Brain brain)
        {
            TrainingRoomId = trainingRoom.Id;
            TrainingRoom = trainingRoom;
            Brain = brain;
        }

        public Organism Crossover(Organism other)
        {
            return new Organism(TrainingRoom, Brain.Crossover(other.Brain));
        }

        public void Mutate()
        {
            Brain.Mutate();
        }

        public Organism Clone()
        {
            return new Organism(TrainingRoom, Brain.Clone());
        }

        internal bool IsSameSpecies(Organism organism)
        {
            return Brain.IsSameSpecies(organism.Brain);
        }
    }
}
