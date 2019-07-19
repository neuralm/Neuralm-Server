using System;
using System.Collections.Generic;
using System.Linq;

namespace Neuralm.Domain.Entities.NEAT
{
    /// <summary>
    /// Represents the <see cref="Species"/> class used for generating <see cref="Organism"/>s.
    /// </summary>
    public class Species
    {
        private List<Organism> _lastGenerationOrganisms;
        private List<Organism> _organisms;

        /// <summary>
        /// Gets and sets the id.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets and sets the training room id.
        /// </summary>
        public Guid TrainingRoomId { get; private set; }

        /// <summary>
        /// Gets the list of organisms.
        /// </summary>
        public virtual IReadOnlyList<Organism> Organisms => _organisms;

        /// <summary>
        /// Gets the list of organisms from the last generation.
        /// </summary>
        public virtual IReadOnlyList<Organism> LastGenerationOrganisms => _lastGenerationOrganisms;

        /// <summary>
        /// Gets and sets the species score.
        /// </summary>
        public double SpeciesScore { get; private set; }

        /// <summary>
        /// EFCore entity constructor IGNORE!
        /// </summary>
        protected Species()
        {

        }

        /// <summary>
        /// Initializes an instance of the <see cref="Species"/> class with the given organism as its first representative.
        /// </summary>
        /// <param name="organism">The organism which this species is created for.</param>
        /// <param name="trainingRoomId">The training room id.</param>
        public Species(Organism organism, Guid trainingRoomId)
        {
            Id = Guid.NewGuid();
            organism.SpeciesId = Id;
            _organisms = new List<Organism> { organism };
            _lastGenerationOrganisms = new List<Organism> { organism };
            TrainingRoomId = trainingRoomId;
        }

        /// <summary>
        /// Adds the organism if it fits this species.
        /// It compares the given organism to a randomly chosen organism from the species.
        /// </summary>
        /// <param name="organism">The organism to check.</param>
        /// <param name="randomNext">The random next.</param>
        /// <returns>Returns <c>true</c> if it is added; otherwise, <c>false</c>.</returns>
        public bool AddOrganismIfSameSpecies(Organism organism, Func<int, int> randomNext)
        {
            if (!GetRandomOrganism(randomNext).IsSameSpecies(organism))
                return false;
            organism.SpeciesId = Id;
            _organisms.Add(organism);
            return true;
        }

        /// <summary>
        /// Kills the worst scoring organisms and gets the species ready for the next generation.
        /// </summary>
        /// <param name="topAmountToSurvive">The top amount percentage to survive.</param>
        public void PostGeneration(double topAmountToSurvive)
        {
            SpeciesScore = Organisms.Sum(organism => organism.Score);

            _organisms.Sort((a, b) =>
            {
                if (a.Score < b.Score)
                    return 1;

                if (a.Score > b.Score)
                    return -1;

                return 0;
            });

            int organismsToSurvive = (int)Math.Ceiling(Organisms.Count * topAmountToSurvive);
            _lastGenerationOrganisms.Clear();
            _lastGenerationOrganisms = Organisms.Take(organismsToSurvive).Select(organism =>
            {
                Organism org = organism.Clone();
                org.Generation++;
                return org;
            }).ToList();
            // TODO: Check if clone is needed, may just be useless instance creation
            _organisms.Clear();
        }

        /// <summary>
        /// Gets a random organism from this species.
        /// </summary>
        /// <param name="randomNext">The random next.</param>
        /// <returns>Returns a randomly chosen <see cref="Organism"/>.</returns>
        public Organism GetRandomOrganism(Func<int, int> randomNext)
        {
            return _lastGenerationOrganisms[randomNext(_lastGenerationOrganisms.Count)];
        }
    }
}
