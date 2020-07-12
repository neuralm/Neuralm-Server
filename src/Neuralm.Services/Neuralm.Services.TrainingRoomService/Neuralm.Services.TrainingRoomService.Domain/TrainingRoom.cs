using Neuralm.Services.Common.Domain;
using Neuralm.Services.Common.Patterns;
using Neuralm.Services.TrainingRoomService.Domain.Exceptions;
using Neuralm.Services.TrainingRoomService.Domain.FactoryArguments;
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
        private readonly IFactory<Organism, OrganismFactoryArgument> _organismFactory;
        private readonly Dictionary<(uint A, uint B), uint> _mutationToInnovation = new Dictionary<(uint A, uint B), uint>();

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
        /// Gets and sets the highest node identifier number.
        /// </summary>
        public uint HighestNodeIdentifier { get; private set; }
        
        /// <summary>
        /// Gets a value indicating whether the training room is enabled.
        /// </summary>
        public bool Enabled { get; private set; }

        /// <summary>
        /// Gets and sets the highest organism score.
        /// </summary>
        public double HighestOrganismScore { get; set; }

        /// <summary>
        /// Gets and sets the lowest organism score.
        /// </summary>
        public double LowestOrganismScore { get; set; }

        /// <summary>
        /// Gets and sets the total score.
        /// </summary>
        public double TotalScore { get; set; }

        /// <summary>
        /// EFCore entity constructor IGNORE!
        /// </summary>
        protected TrainingRoom()
        {
            // Preset here since all dto's do not have species defined in them.
            Species = new List<Species>();
            // Since it is the default domain the factory can be constructed here.
            _organismFactory = new OrganismFactory();
        }

        /// <summary>
        /// Initializes an instance of the <see cref="TrainingRoom"/> class with the given settings.
        /// The training room manages all of the organisms and the settings.
        /// </summary>
        /// <param name="id">The training room id.</param>
        /// <param name="owner">The user who created this training room.</param>
        /// <param name="name">The name for the room.</param>
        /// <param name="trainingRoomSettings">The settings for this training room.</param>
        /// <param name="organismFactory">The organism factory.</param>
        public TrainingRoom(Guid id, User owner, string name, TrainingRoomSettings trainingRoomSettings, IFactory<Organism, OrganismFactoryArgument> organismFactory)
        {
            _organismFactory = organismFactory;
            Id = id;
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
                if (species.StagnantCounter >= TrainingRoomSettings.MaxStagnantTime || !species.IsSameSpecies(organism, TrainingRoomSettings))
                    continue;
                species.AddOrganism(organism);
                return;
            }

            // If the organism does not belong to any of the species create a new species and add it to the species.
            Species.Add(new Species(organism, Id));
        }

        // FIXME: Require Species to have an IsEvaluated property
        /// <summary>
        /// Loops over all organisms in all species in the current generation to check if they are evaluated or not. 
        /// If one or more aren't evaluated false is returned.
        /// </summary>
        public bool AllOrganismsInCurrentGenerationAreEvaluated()
            => Species.SelectMany(species => species.Organisms.Where(o => o.Generation == Generation)).All(o => o.IsEvaluated);

        /// <summary>
        /// Does 1 generation.
        /// kills the worst ones, mutate and breed and make the system ready for a new generation.
        /// </summary>
        /// <param name="markOrganismForRemoval">The action to mark organisms for removal.</param>
        /// <param name="markSpeciesForRemoval">The action to mark species for removal.</param>
        public void EndGeneration(Action<Organism> markOrganismForRemoval, Action<Species> markSpeciesForRemoval)
        {
            // Verifies that all organisms of the current generation are evaluated, otherwise throw an exception.
            if (!AllOrganismsInCurrentGenerationAreEvaluated())
                throw new UnevaluatedOrganismException("An organism is not evaluated, but end generation was called!");

            // Prepares score values.
            HighestOrganismScore = double.MinValue;
            LowestOrganismScore = double.MaxValue;
            TotalScore = 0;

            // For each through all the species.
            foreach (Species species in Species)
            {
                // Calculate the highest and lowest score.
                foreach (Organism organism in species.Organisms)
                {
                    organism.Score /= Species.Count;
                    HighestOrganismScore = Math.Max(organism.Score, HighestOrganismScore);
                    LowestOrganismScore = Math.Min(organism.Score, LowestOrganismScore);
                }

                // Reproduce with the given training room settings.
                species.PostGeneration(TrainingRoomSettings.TopAmountToSurvive, Generation, markOrganismForRemoval);

                // Calculate the total score for all species.
                TotalScore += species.SpeciesScore;
            }

            // If the total score is 0 than force the total score to be 1.
            if (TotalScore == 0)
                TotalScore = 1; // TODO: Think about what this really does lol, if the total score is 0 should they have the right to reproduce?

            // Prepare rest value.
            double rest = 0;

            // Prepare total organisms value.
            double totalOrganisms = 0;

            // For each species determine the amount of organisms that is allowed to survive
            foreach (Species species in Species)
            {
                // If the species is stagnant don't let it reproduce 
                if (species.StagnantCounter >= TrainingRoomSettings.MaxStagnantTime)
                {
                    markSpeciesForRemoval(species);
                    species.Organisms.Clear();
                    continue;
                }
                
                double fraction = species.SpeciesScore / TotalScore;
                double amountOfOrganisms = TrainingRoomSettings.OrganismCount * fraction;
                rest += amountOfOrganisms % 1;

                if (rest >= 1)
                {
                    amountOfOrganisms++;
                    rest--;
                }

                amountOfOrganisms = Math.Floor(amountOfOrganisms);

                if (amountOfOrganisms > 1 && species.Organisms.Count > TrainingRoomSettings.ChampionCloneMinSpeciesSize)
                {
                    amountOfOrganisms--;
                    totalOrganisms++;
                    species.AddOrganism(species.GetChampion().Clone(TrainingRoomSettings));
                }
                
                for (int i = 0; i < amountOfOrganisms; i++)
                {
                    species.AddOrganism(ProduceOrganism(species));
                }
                totalOrganisms += amountOfOrganisms;
            }

            // If the total organisms count is lower than the amount specified in the training room settings then add more.
            while (totalOrganisms < TrainingRoomSettings.OrganismCount)
            {
                AddOrganism(_organismFactory.Create(new OrganismFactoryArgument()
                {
                    TrainingRoomSettings = TrainingRoomSettings, 
                    Generation = Generation + 1,
                    InnovationFunction = GetInnovationNumber
                }));
                totalOrganisms++;
            }

            // Increases the generation.
            Generation++;

            // Removes any species that have died out.
            Species.RemoveAll(species => !species.Organisms.Any());

            // Resets the mutation to innovation map.
            _mutationToInnovation.Clear();
        }

        /// <summary>
        /// Increases the nodeID to the value passed in; If the nodeId is lower than the value passed in.
        /// </summary>
        /// <param name="min">The minimum value the nodeId should be.</param>
        public void IncreaseNodeIdTo(uint min)
        {
            HighestNodeIdentifier = Math.Max(HighestNodeIdentifier, min);
        }

        /// <summary>
        /// Gets the node id and increases it.
        /// </summary>
        /// <returns>Returns the old node id before increasing it.</returns>
        public uint GetAndIncreaseNodeId()
        {
            uint temp = HighestNodeIdentifier;
            HighestNodeIdentifier += 1;
            return temp;
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
            Organism temp;

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
                temp = child.Crossover(parent2, TrainingRoomSettings, _organismFactory);
            }
            else
            {
                // Clone the current child.
                temp = child.Clone(TrainingRoomSettings);

                // Increment generation, crossover does that automatically, but if no crossover happens we need to do it here.
                temp.Generation++;
            }

            // If the random value is lower than the training room settings mutation chance,
            // mutate the child with the training room settings.
            if (TrainingRoomSettings.Random.NextDouble() < TrainingRoomSettings.MutationChance)
                temp.Mutate(TrainingRoomSettings, GetAndIncreaseNodeId, GetInnovationNumber);

            // Return the child.
            return temp;
        }

        public override string ToString()
        {
            return
                $"Id: {Id}, OwnerId: {OwnerId}, AuthorizedTrainers: {AuthorizedTrainers.Count}, TrainingSessions: {TrainingSessions}, Species: {Species.Count}, " +
                $"Name: {Name}, Generation: {Generation}, HighestInnovationNumber: {HighestInnovationNumber}, Enabled: {Enabled}";
        }
    }
}
