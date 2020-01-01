using Neuralm.Services.Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Neuralm.Services.TrainingRoomService.Domain
{
    /// <summary>
    /// Represents the <see cref="TrainingRoom"/> class; provides methods for managing training sessions.
    /// </summary>
    public class TrainingRoom : IEntity
    {
        private readonly Dictionary<(uint A, uint B), uint> _mutationToInnovation = new Dictionary<(uint A, uint B), uint>();
        private readonly List<Organism> _tempOrganisms = new List<Organism>();
        private uint _nodeIdentifier;

        /// <summary>
        /// Gets and sets the id.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets and sets the owner id.
        /// </summary>
        public Guid OwnerId { get; private set; }
        
        /// <summary>
        /// Gets and sets the owner.
        /// </summary>
        public virtual User Owner { get; private set; }

        /// <summary>
        /// Gets the list of authorized trainers.
        /// </summary>
        public virtual List<Trainer> AuthorizedTrainers { get; private set; }

        /// <summary>
        /// Gets the list of training sessions.
        /// </summary>
        public virtual List<TrainingSession> TrainingSessions { get; private set; }

        /// <summary>
        /// Gets the list of species.
        /// </summary>
        public virtual List<Species> Species { get; private set; }

        /// <summary>
        /// Gets and sets the training room settings.
        /// </summary>
        public virtual TrainingRoomSettings TrainingRoomSettings { get; private set; }

        /// <summary>
        /// Gets and sets the name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets and sets the generation.
        /// </summary>
        public uint Generation { get; private set; }

        /// <summary>
        /// Gets and sets the highest innovation number.
        /// </summary>
        public uint HighestInnovationNumber { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the training room is enabled.
        /// </summary>
        public bool Enabled { get; private set; }

        /// <summary>
        /// EFCore entity constructor IGNORE!
        /// </summary>
        protected TrainingRoom()
        {

        }

        /// <summary>
        /// Initializes an instance of the <see cref="TrainingRoom"/> class with the given settings.
        /// The training room manages all of the organisms and the settings.
        /// </summary>
        /// <param name="owner">The user who created this training room.</param>
        /// <param name="name">The name for the room.</param>
        /// <param name="trainingRoomSettings">The settings for this training room.</param>
        public TrainingRoom(User owner, string name, TrainingRoomSettings trainingRoomSettings)
        {
            Id = Guid.NewGuid();
            Name = name;
            Owner = owner;
            OwnerId = owner.Id;
            Generation = 0;
            Enabled = true;
            TrainingRoomSettings = trainingRoomSettings;
            AuthorizedTrainers = new List<Trainer> { new Trainer(owner, this) };
            Species = new List<Species>();
            TrainingSessions = new List<TrainingSession>();
        }

        /// <summary>
        /// Adds a organism to the training room.
        /// Checks each species and creates a new species if no species matches.
        /// </summary>
        /// <param name="organism">The organism to add.</param>
        public void AddOrganism(Organism organism)
        {
            // For each species in the species check if the organism is of the same species
            // and add it if true.
            foreach (Species species in Species)
            {
                if (!species.IsSameSpecies(organism, TrainingRoomSettings))
                    continue;
                species.Organisms.Add(organism);
                return;
            }

            // If the organism does not belong to any of the species create a new species and add it to the species.
            Species.Add(new Species(organism, Id));
        }

        /// <summary>
        /// Sets the organism's scores to what the client sends.
        /// </summary>
        /// <param name="organism">The organism.</param>
        /// <param name="score">The score.</param>
        public void PostScore(Organism organism, double score)
        {
            organism.Score = score;
            organism.Evaluated = true;
        }

        /// <summary>
        /// Does 1 generation.
        /// kills the worst ones, mutate and breed and make the system ready for a new generation.
        /// </summary>
        public bool EndGeneration()
        {
            // Verifies that all organisms of the current generation are evaluated, otherwise return false.
            if (!Species.TrueForAll(s => s.Organisms.Where(o => o.Generation == Generation).All(o => o.Evaluated)))
                return false;

            // Prepares score values.
            double highestScore = double.MinValue;
            double lowestScore = double.MaxValue;

            // For each through all the species and calculate the highest and lowest score.
            foreach (Species species in Species)
            {
                // Remove all previous generation organisms, now only current generation organisms should exist.
                species.Organisms.RemoveAll(o => o.Generation < Generation);
                foreach (Organism organism in species.Organisms)
                {
                    organism.Score /= Species.Count;
                    highestScore = Math.Max(organism.Score, highestScore);
                    lowestScore = Math.Min(organism.Score, lowestScore);
                }
            }

            // For each species reproduce with the given training room settings.
            foreach (Species species in Species)
            {
                species.PostGeneration(TrainingRoomSettings.TopAmountToSurvive, Generation);
            }

            // Calculate the total score for all species.
            double totalScore = Species.Sum(species => species.SpeciesScore);

            // If the total score is 0 than force the total score to be 1.
            if (totalScore == 0)
                totalScore = 1; // TODO: Think about what this really does lol, if the total score is 0 should they have the right to reproduce?

            // Prepare rest value.
            double rest = 0;

            // For each species determine the amount of organisms that is allowed to survive 
            // and add them to the temporary organisms list.
            foreach (Species species in Species)
            {
                double fraction = species.SpeciesScore / totalScore;
                double amountOfOrganisms = TrainingRoomSettings.OrganismCount * fraction;
                rest += amountOfOrganisms % 1;

                if (rest >= 1)
                {
                    amountOfOrganisms++;
                    rest--;
                }

                amountOfOrganisms = Math.Floor(amountOfOrganisms);
                for (int i = 0; i < amountOfOrganisms; i++)
                {
                    _tempOrganisms.Add(ProduceOrganism(species));
                }
            }

            // If the temporary organisms count is lower than the amount specified in the training room settings then add more.
            while (_tempOrganisms.Count < TrainingRoomSettings.OrganismCount)
            {
                _tempOrganisms.Add(new Organism(Generation + 1, TrainingRoomSettings));
            }

            // Adds the organisms from the temporary list to the species list.
            foreach (Organism organism in _tempOrganisms)
            {
                AddOrganism(organism);
            }

            // Clears the temporary organisms list.
            _tempOrganisms.Clear();

            // Increases the generation.
            Generation++;

            // Remove all organisms that are not in the current generation.
            Species.ForEach(s => s.Organisms.RemoveAll(o => o.Generation < Generation));

            // Removes any species that have died out.
            Species.RemoveAll(species => !species.Organisms.Any());

            // Resets the mutation to innovation map.
            _mutationToInnovation.Clear();

            // Returns true for a successful generation.
            return true;
        }

        /// <summary>
        /// Increases the nodeID to the value passed in; If the nodeId is lower than the value passed in.
        /// </summary>
        /// <param name="min">The minimum value the nodeId should be.</param>
        public void IncreaseNodeIdTo(uint min)
        {
            _nodeIdentifier = Math.Max(_nodeIdentifier, min);
        }

        /// <summary>
        /// Gets the node id and increases it.
        /// </summary>
        /// <returns>Returns the old node id before increasing it.</returns>
        public uint GetAndIncreaseNodeId()
        {
            return _nodeIdentifier++;
        }

        /// <summary>
        /// Gets the innovation number that corresponds with the given in and out node id.
        /// </summary>
        /// <param name="inId">The inId of the new connection.</param>
        /// <param name="outId">The outId of the new connection.</param>
        /// <returns>Returns the innovation number.</returns>
        public uint GetInnovationNumber(uint inId, uint outId)
        {
            if (_mutationToInnovation.ContainsKey((inId, outId)))
                return _mutationToInnovation[(inId, outId)];

            _mutationToInnovation.Add((inId, outId), ++HighestInnovationNumber);
            return HighestInnovationNumber;
        }

        /// <summary>
        /// Sets Enabled to <c>false</c>.
        /// </summary>
        public void Disable()
        {
            Enabled = false;
        }

        /// <summary>
        /// Sets Enabled to <c>true</c>.
        /// </summary>
        public void Enable()
        {
            // TODO: Is within re-enable period?
            Enabled = true;
        }

        /// <summary>
        /// Authorizes a user for the training room.
        /// </summary>
        /// <param name="user">The user to authorize.</param>
        /// <returns>Returns <c>true</c> if the user is added to the authorized users; otherwise, <c>false</c>.</returns>
        public bool AuthorizeUser(User user)
        {
            if (AuthorizedTrainers.Exists(usr => usr.UserId.Equals(user.Id)))
                return false;
            AuthorizedTrainers.Add(new Trainer(user, this));
            return true;
        }

        /// <summary>
        /// Deauthorizes a user for the training room.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>Returns <c>true</c> if the user is removed from the authorized users; otherwise, <c>false</c>.</returns>
        public bool DeauthorizeUser(Guid userId)
        {
            Trainer possibleUser = AuthorizedTrainers.SingleOrDefault(usr => usr.UserId.Equals(userId));
            return possibleUser != default && AuthorizedTrainers.Remove(possibleUser);
        }

        /// <summary>
        /// Checks if the given user id is authorized.
        /// </summary>
        /// <param name="userId">The user id to verify.</param>
        /// <returns>Returns <c>true</c> if the given user id is authorized; otherwise, <c>false</c>.</returns>
        public bool IsUserAuthorized(Guid userId)
        {
            return AuthorizedTrainers.Exists(user => user.UserId.Equals(userId));
        }

        /// <summary>
        /// Starts the training session for the given user id.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="trainingSession">The training session.</param>
        /// <returns>Returns <c>true</c> if the training session is started; otherwise, <c>false</c>.</returns>
        public bool StartTrainingSession(Guid userId, out TrainingSession trainingSession)
        {
            trainingSession = new TrainingSession(this, userId);
            if (TrainingSessions.Any(ts => ts.UserId.Equals(userId)))
                return false;
            TrainingSessions.Add(trainingSession);
            return true;
        }

        /// <summary>
        /// Generates an organism based on the organisms from this species.
        /// A random organism is chosen and based on chance it can be bred with a random organism from this species or from the global pool.
        /// The organism is also mutated.
        /// </summary>
        /// <param name="species">The species.</param>
        /// <returns>Returns a generated <see cref="Organism"/>.</returns>
        private Organism ProduceOrganism(Species species)
        {
            // Gets a random organism as child.
            Organism child = species.GetRandomOrganism(Generation, TrainingRoomSettings);

            // If the random value is lower than the training room settings cross over chance,
            // perform a cross over. Otherwise, clone the child.
            if (TrainingRoomSettings.Random.NextDouble() < TrainingRoomSettings.CrossOverChance)
            {
                // Depending on the training room settings for the inter species chance to cross over get the 
                // random organism from a random species else get it from the given species.
                Organism parent2 = TrainingRoomSettings.Random.NextDouble() < TrainingRoomSettings.InterSpeciesChance
                    ? Species[TrainingRoomSettings.Random.Next(Species.Count)].GetRandomOrganism(Generation, TrainingRoomSettings)
                    : species.GetRandomOrganism(Generation, TrainingRoomSettings);

                // Cross over the two organisms with the training room settings.
                child = child.Crossover(parent2, TrainingRoomSettings);
            }
            else
            {
                // Clone the current child.
                child = child.Clone(TrainingRoomSettings);

                // Increment generation, crossover does that automatically, but if no crossover happens we need to do it here.
                child.Generation++;
            }

            // If the random value is lower than the training room settings mutation chance,
            // mutate the child with the training room settings.
            if (TrainingRoomSettings.Random.NextDouble() < TrainingRoomSettings.MutationChance)
                child.Mutate(TrainingRoomSettings, GetAndIncreaseNodeId, GetInnovationNumber);

            // Return the child.
            return child;
        }
    }
}
