using Neuralm.Services.Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Neuralm.Services.TrainingRoomService.Domain
{
    /// <summary>
    /// Represents the <see cref="Species"/> class used for generating <see cref="Organism"/>s.
    /// </summary>
    public class Species : IEntity
    {
        /// <summary>
        /// Gets and sets the id.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets the list of organisms.
        /// </summary>
        public virtual List<Organism> Organisms { get; private set; }

        /// <summary>
        /// Gets and sets the species score.
        /// </summary>
        public double SpeciesScore { get; private set; }
        
        /// <summary>
        /// Gets and sets the training room id.
        /// </summary>
        public Guid TrainingRoomId { get; set; }
        
        /// <summary>
        /// EFCore entity constructor IGNORE!
        /// </summary>
        protected Species()
        {
            Organisms = new List<Organism>();
        }

        /// <summary>
        /// Initializes an instance of the <see cref="Species"/> class with the given organism as its first representative.
        /// </summary>
        /// <param name="organism">The organism which this species is created for.</param>
        /// <param name="trainingRoomId">The training room id.</param>
        public Species(Organism organism, Guid trainingRoomId)
        {
            // Generates a new guid for the species, this is needed so EF can set the foreign shadow key: SpeciesId, on Organism.
            Id = Guid.NewGuid();

            // Adds the organism to the species.
            Organisms = new List<Organism> { organism };
            
            // Initialize the training room id.
            TrainingRoomId = trainingRoomId;
        }

        /// <summary>
        /// Checks if the organism is of the same species.
        /// It compares the given organism to a randomly chosen organism from the species.
        /// </summary>
        /// <param name="organism">The organism to check.</param>
        /// <param name="trainingRoomSettings">The training rooms settings.</param>
        /// <returns>Returns <c>true</c> if he organism is of the same species; otherwise, <c>false</c>.</returns>
        internal bool IsSameSpecies(Organism organism, TrainingRoomSettings trainingRoomSettings)
        {
            // If the generation is 0, only 1 species should exist.
            // So, there is no need check for the same species.
            if (organism.Generation == 0)
                return true;

            // Get all organisms from the previous generation.
            List<Organism> lastGenOrganisms = Organisms.FindAll(org => org.Generation == organism.Generation - 1);

            // If the last generation does not contain any organisms, that indicates that the species is new.
            if (!lastGenOrganisms.Any())
            {
                // The new species will only have organisms from the current generation.
                // So, we make the current organisms the representatives of the last generation.
                lastGenOrganisms = Organisms;
            }

            // Gets the randomly selected organism from the last generation organisms.
            Organism organismToCompare = lastGenOrganisms[trainingRoomSettings.Random.Next(lastGenOrganisms.Count)];

            // Compares if the organism is from the same species.
            return organismToCompare.IsSameSpecies(organism, trainingRoomSettings);
        }

        /// <summary>
        /// Kills the worst scoring organisms and gets the species ready for the next generation.
        /// </summary>
        /// <param name="topAmountToSurvive">The top amount percentage to survive.</param>
        /// <param name="generation">The current generation.</param>
        internal void PostGeneration(double topAmountToSurvive, uint generation)
        {
            // Remove all organisms from the last generation.
            Organisms.RemoveAll(o => o.Generation == generation - 1);

            // Sum all of the scores of the current generation.
            SpeciesScore = Organisms.Sum(organism => organism.Score);

            // Sort the organisms to make sure they are in other from good to bad.
            Organisms.Sort((a, b) =>
            {
                if (a.Score < b.Score)
                    return 1;

                if (a.Score > b.Score)
                    return -1;

                return 0;
            });

            // Calculate how many organisms should survive, these will later reproduce so it doesn't matter
            // if there are too many surviving (eg 1.5 > 2). But, we want to make sure at least 1 survives.
            int organismsToSurvive = (int)Math.Ceiling(Organisms.Count * topAmountToSurvive);

            // Sets the current organisms to a certain amount of the top organisms that have been sorted before.
            Organisms = Organisms.Take(organismsToSurvive).ToList();
        }

        /// <summary>
        /// Gets a random organism from this species.
        /// </summary>
        /// <param name="generation">The current generation.</param>
        /// <param name="trainingRoomSettings">The training room settings.</param>
        /// <returns>Returns a randomly chosen <see cref="Organism"/>.</returns>
        internal Organism GetRandomOrganism(uint generation, TrainingRoomSettings trainingRoomSettings)
        {
            // Gets the current generation organisms.
            List<Organism> currentGen = Organisms.FindAll(o => o.Generation == generation);

            // Return the only organism or a random organism.
            if (currentGen.Count == 1)
                return currentGen.First();
            if (currentGen.Count > 1)
                return currentGen[trainingRoomSettings.Random.Next(currentGen.Count)];

            // Should never happen!
            throw new Exception($"The organisms does not contain a organism at the generation: {generation}");
        }
    }
}
