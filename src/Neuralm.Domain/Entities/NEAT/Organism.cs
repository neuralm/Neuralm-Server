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

        public String Name { get; internal set; }

        Organism() { }

        public Organism(TrainingRoom trainingRoom) : this(trainingRoom, new Brain(trainingRoom))
        {
        }

        Organism(TrainingRoom trainingRoom, Brain brain)
        {
            TrainingRoomId = trainingRoom.Id;
            TrainingRoom = trainingRoom;
            Brain = brain;
            Name = GenerateName(trainingRoom.Random.Next);
        }

        static string[] vowels = new string[] { "a", "e", "i", "o", "u", "y", "aa", "ee", "ie", "oo", "ou", "au" };
        static string[] consonants = new string[] { "b", "c", "f", "g", "h", "j", "k", "l", "m", "n", "p", "q", "r", "s", "t", "v", "w", "x", "z" };

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
