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
        private List<Brain> _brains;

        /// <summary>
        /// Gets and sets the id.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets the list of brains.
        /// </summary>
        public virtual IReadOnlyList<Brain> Brains => _brains;

        /// <summary>
        /// Gets the list of brains from the last generation.
        /// </summary>
        public virtual IReadOnlyList<Brain> LastGenerationBrains => _lastGenerationBrains;

        /// <summary>
        /// Gets and sets the species score.
        /// </summary>
        public double SpeciesScore { get; private set; }

        /// <summary>
        /// Gets and sets the training room id.
        /// </summary>
        public Guid TrainingRoomId { get; private set; }

        /// <summary>
        /// EFCore entity constructor IGNORE!
        /// </summary>
        protected Species()
        {

        }

        /// <summary>
        /// Initializes an instance of the <see cref="Species"/> class with the given brain as its first representative.
        /// </summary>
        /// <param name="brain">The brain which this species is created for.</param>
        /// <param name="trainingRoomId">The training room id.</param>
        public Species(Brain brain, Guid trainingRoomId)
        {
            Id = Guid.NewGuid();
            brain.SpeciesId = Id;
            _brains = new List<Brain> { brain };
            _lastGenerationBrains = new List<Brain> { brain };
            TrainingRoomId = trainingRoomId;
        }

        /// <summary>
        /// Adds the brain if it fits this species.
        /// It compares the given brain to a randomly chosen brain from the species.
        /// </summary>
        /// <param name="brain">The brain to check.</param>
        /// <param name="randomNext">The random next.</param>
        /// <returns>Returns <c>true</c> if it is added; otherwise, <c>false</c>.</returns>
        public bool AddBrainIfSameSpecies(Brain brain, Func<int, int> randomNext)
        {
            if (!GetRandomBrain(randomNext).IsSameSpecies(brain))
                return false;
            brain.SpeciesId = Id;
            _brains.Add(brain);
            return true;
        }

        /// <summary>
        /// Kills the worst scoring brains and gets the species ready for the next generation.
        /// </summary>
        /// <param name="topAmountToSurvive">The top amount to survive.</param>
        public void PostGeneration(double topAmountToSurvive)
        {
            SpeciesScore = Brains.Sum(brain => brain.Score);

            _brains.Sort((a, b) =>
            {
                if (a.Score < b.Score)
                    return 1;

                if (a.Score > b.Score)
                    return -1;

                return 0;
            });

            int brainsToSurvive = (int)Math.Ceiling(Brains.Count * topAmountToSurvive);

            _lastGenerationBrains = Brains.Take(brainsToSurvive).Select(brain => brain.Clone()).ToList();
            // TODO: Check if clone is needed, may just be useless instance creation
            _brains.Clear();
        }

        /// <summary>
        /// Gets a random brain from this species.
        /// </summary>
        /// <param name="randomNext">The random next.</param>
        /// <returns>Returns a randomly chosen <see cref="Brain"/>.</returns>
        public Brain GetRandomBrain(Func<int, int> randomNext)
        {
            return _lastGenerationBrains[randomNext(_lastGenerationBrains.Count)];
        }
    }
}
