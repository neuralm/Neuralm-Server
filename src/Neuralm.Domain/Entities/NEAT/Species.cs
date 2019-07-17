using System;
using System.Collections.Generic;
using System.Linq;

namespace Neuralm.Domain.Entities.NEAT
{
    /// <summary>
    /// Represents the <see cref="Species"/> class used for generating <see cref="Brain"/>s.
    /// </summary>
    public class Species
    {
        private List<Brain> _lastGenerationBrains;
        private readonly TrainingRoom _trainingRoom;

        /// <summary>
        /// Gets the list of brains.
        /// </summary>
        public List<Brain> Brains { get; }

        /// <summary>
        /// Gets and sets the species score.
        /// </summary>
        public double SpeciesScore { get; private set; }

        /// <summary>
        /// Initializes an instance of the <see cref="Species"/> class with the given brain as its first representative.
        /// </summary>
        /// <param name="brain">The brain which this species is created for.</param>
        /// <param name="trainingRoom">The training room this species is part of.</param>
        public Species(Brain brain, TrainingRoom trainingRoom)
        {
            _trainingRoom = trainingRoom;
            Brains = new List<Brain> { brain };
            _lastGenerationBrains = new List<Brain> { brain };
        }

        /// <summary>
        /// Adds the brain if it fits this species.
        /// It compares the given brain to a randomly chosen brain from the species.
        /// </summary>
        /// <param name="brain">The brain to check.</param>
        /// <returns>Returns <c>true</c> if it is added; otherwise, <c>false</c>.</returns>
        public bool AddBrainIfSameSpecies(Brain brain)
        {
            if (!GetRandomBrain().IsSameSpecies(brain))
                return false;
            Brains.Add(brain);
            return true;
        }

        /// <summary>
        /// Generates a brain based on the brains from this species.
        /// A random brain is chosen and based on chance it can be bred with a random brain from this species or from the global pool.
        /// The brain is also mutated.
        /// </summary>
        /// <returns>Returns a generated <see cref="Brain"/>.</returns>
        public Brain ProduceBrain()
        {
            Brain child = GetRandomBrain();
            if (_trainingRoom.Random.NextDouble() < _trainingRoom.TrainingRoomSettings.CrossOverChance)
            {
                Brain parent2 = _trainingRoom.Random.NextDouble() < _trainingRoom.TrainingRoomSettings.InterSpeciesChance 
                    ? _trainingRoom.GetRandomBrain() 
                    : GetRandomBrain();

                child = child.Crossover(parent2);
            }
            else
                child = child.Clone();

            if(_trainingRoom.Random.NextDouble() < _trainingRoom.TrainingRoomSettings.MutationChance)
                child.Mutate();

            return child;
        }

        /// <summary>
        /// Kills the worst scoring brains and gets the species ready for the next generation.
        /// </summary>
        public void PostGeneration()
        {
            SpeciesScore = Brains.Sum(brain => brain.Score);

            Brains.Sort((a, b) =>
            {
                if (a.Score < b.Score)
                    return 1;

                if (a.Score > b.Score)
                    return -1;

                return 0;
            });

            int brainsToSurvive = (int)Math.Ceiling(Brains.Count * _trainingRoom.TrainingRoomSettings.TopAmountToSurvive);

            _lastGenerationBrains = Brains.Take(brainsToSurvive).Select(brain => brain.Clone()).ToList();
             // TODO: Check if clone is needed, may just be useless instance creation
            Brains.Clear();
        }

        /// <summary>
        /// Gets a random brain from this species.
        /// </summary>
        /// <returns>Returns a randomly chosen <see cref="Brain"/>.</returns>
        public Brain GetRandomBrain()
        {
            return _lastGenerationBrains[_trainingRoom.Random.Next(_lastGenerationBrains.Count)];
        }
    }
}
