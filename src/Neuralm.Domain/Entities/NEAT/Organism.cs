using System;

namespace Neuralm.Domain.Entities.NEAT
{
    /// <summary>
    /// Represents the <see cref="Organism"/> class.
    /// </summary>
    public class Organism
    {
        private static readonly string[] Vowels = { "a", "e", "i", "o", "u", "y", "aa", "ee", "ie", "oo", "ou", "au" };
        private static readonly string[] Consonants = { "b", "c", "f", "g", "h", "j", "k", "l", "m", "n", "p", "q", "r", "s", "t", "v", "w", "x", "z" };


        /// <summary>
        /// Gets and sets the id.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets and sets the species id.
        /// </summary>
        public Guid SpeciesId { get; set; }

        /// <summary>
        /// Gets and sets the brain id.
        /// </summary>
        public Guid BrainId { get; private set; }

        /// <summary>
        /// Gets and sets the training room id.
        /// </summary>
        public Guid TrainingRoomId { get; private set; }

        /// <summary>
        /// Gets and sets the brain.
        /// </summary>
        public virtual Brain Brain { get; private set; }

        /// <summary>
        /// Gets and sets the training room.
        /// </summary>
        public virtual TrainingRoom TrainingRoom { get; private set; }

        /// <summary>
        /// Gets and sets the score.
        /// </summary>
        public double Score { get; internal set; }

        /// <summary>
        /// Gets and sets the name.
        /// </summary>
        public String Name { get; internal set; }

        /// <summary>
        /// EFCore entity constructor IGNORE!
        /// </summary>
        protected Organism()
        {

        }

        /// <summary>
        /// Initializes an instance of the <see cref="Organism"/> class.
        /// </summary>
        /// <param name="trainingRoom">The training room.</param>
        public Organism(TrainingRoom trainingRoom)
        {
            Id = Guid.NewGuid();
            TrainingRoomId = trainingRoom.Id;
            TrainingRoom = trainingRoom;
            Brain = new Brain(trainingRoom);
            BrainId = Brain.Id;
            Name = GenerateName(trainingRoom.Random.Next);
        }

        /// <summary>
        /// Initializes an instance of the <see cref="Organism"/> class.
        /// </summary>
        /// <param name="trainingRoom">The training room.</param>
        /// <param name="brain">The brain.</param>
        private Organism(TrainingRoom trainingRoom, Brain brain)
        {
            Id = Guid.NewGuid();
            TrainingRoomId = trainingRoom.Id;
            TrainingRoom = trainingRoom;
            Brain = brain;
            BrainId = brain.Id;
            Name = GenerateName(trainingRoom.Random.Next);
        }

        /// <summary>
        /// Generates a random name.
        /// </summary>
        /// <param name="randomNext">The function that generates a random number between 0 and x.</param>
        /// <returns>Returns a random generated name.</returns>
        private static string GenerateName(Func<int, int> randomNext)
        {
            string name = Consonants[randomNext(Consonants.Length)];

            name += Vowels[randomNext(Vowels.Length)];

            return randomNext(name.Length) == 0
                ? name + GenerateName(randomNext)
                : name + Consonants[randomNext(Consonants.Length)];
        }

        /// <summary>
        /// Crosses over the other organism with this one.
        /// </summary>
        /// <param name="other">The other organism.</param>
        /// <returns>Returns a new organism.</returns>
        public Organism Crossover(Organism other)
        {
            return new Organism(TrainingRoom, Brain.Crossover(other.Brain));
        }

        /// <summary>
        /// Mutates the brain of the organism.
        /// </summary>
        public void Mutate()
        {
            Brain.Mutate();
        }

        /// <summary>
        /// Clones the organism.
        /// </summary>
        /// <returns>Returns a clone of the organism.</returns>
        public Organism Clone()
        {
            return new Organism(TrainingRoom, Brain.Clone());
        }

        /// <summary>
        /// Checks if the other organism is of the same species.
        /// </summary>
        /// <param name="organism">The other organism.</param>
        /// <returns>Returns <c>true</c> if the other organism is of the same species; otherwise, <c>false</c>.</returns>
        public bool IsSameSpecies(Organism organism)
        {
            return Brain.IsSameSpecies(organism.Brain);
        }
    }
}
