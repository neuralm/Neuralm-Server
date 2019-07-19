using System;

namespace Neuralm.Domain.Entities.NEAT
{
    /// <summary>
    /// Represents the <see cref="Organism"/> class.
    /// </summary>
    public class Organism
    {
        private static readonly string[] vowels = new string[] { "a", "e", "i", "o", "u", "y", "aa", "ee", "ie", "oo", "ou", "au" };
        private static readonly string[] consonants = new string[] { "b", "c", "f", "g", "h", "j", "k", "l", "m", "n", "p", "q", "r", "s", "t", "v", "w", "x", "z" };


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
        /// Generate a random name
        /// </summary>
        /// <param name="randomNext">The function that generates a random number between 0 and x</param>
        /// <returns>A random string</returns>
        public static string GenerateName(Func<int, int> randomNext)
        {
            string name = consonants[randomNext(consonants.Length)];

            name += vowels[randomNext(vowels.Length)];

            if(randomNext(name.Length)==0)
            {
                return name + GenerateName(randomNext);
            } else
            {
                return name + consonants[randomNext(consonants.Length)]; 
            }
       
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
