using System;
using System.Collections.Generic;
using System.Linq;

namespace Neuralm.Domain.Entities.NEAT
{
    /// <summary>
    /// Represents the <see cref="TrainingRoom"/> class; provides methods for managing training sessions.
    /// </summary>
    public class TrainingRoom
    {
        private readonly Dictionary<(uint A, uint B), uint> _mutationToInnovation;
        private uint _nodeId;
        private List<User> _authorizedUsers;
        private List<TrainingSession> _trainingSessions;
        private List<Organism> _organisms;
        private List<Species> _species;

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
        /// Gets the list of authorized users.
        /// </summary>
        public virtual IReadOnlyList<User> AuthorizedUsers => _authorizedUsers;

        /// <summary>
        /// Gets the list of training sessions.
        /// </summary>
        public virtual IReadOnlyList<TrainingSession> TrainingSessions => _trainingSessions;

        /// <summary>
        /// Gets the list of organisms.
        /// </summary>
        public virtual IReadOnlyList<Organism> Organisms => _organisms;

        /// <summary>
        /// Gets the list of species.
        /// </summary>
        public virtual IReadOnlyList<Species> Species => _species;

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
        /// Gets and sets the random.
        /// </summary>
        public Random Random { get; private set; }

        /// <summary>
        /// Gets and sets the highest score.
        /// </summary>
        public double HighestScore { get; private set; }

        /// <summary>
        /// Gets and sets the lowest score.
        /// </summary>
        public double LowestScore { get; private set; }

        /// <summary>
        /// Gets and sets the average score.
        /// </summary>
        public double AverageScore { get; private set; }
        
        /// <summary>
        /// Gets and sets the innovation id.
        /// </summary>
        public uint InnovationId { get; private set; }
        
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
            Generation = 0;
            Owner = owner;
            OwnerId = Owner.Id;
            Name = name;
            Enabled = true;
            TrainingRoomSettings = trainingRoomSettings;
            Random = new Random(trainingRoomSettings.Seed);
            _authorizedUsers = new List<User> { owner };
            _trainingSessions = new List<TrainingSession>();
            _species = new List<Species>((int)trainingRoomSettings.OrganismCount);
            _organisms = new List<Organism>((int)trainingRoomSettings.OrganismCount);
            _mutationToInnovation = new Dictionary<(uint A, uint B), uint>();

            for (int i = 0; i < trainingRoomSettings.OrganismCount; i++)
            {
                AddOrganism(new Organism(this));
            }
        }

        /// <summary>
        /// Adds a organism to the training room.
        /// Checks each species and creates a new species if no species matches.
        /// </summary>
        /// <param name="organism">The organism to add.</param>
        public void AddOrganism(Organism organism)
        {
            foreach (Species species in _species)
            {
                if (species.AddOrganismIfSameSpecies(organism, Random.Next))
                {
                    return;
                }
            }

            _species.Add(new Species(organism, Id));
        }

        /// <summary>
        /// Gets a random organism from a random species.
        /// </summary>
        /// <returns>A randomly chosen <see cref="Organism"/>.</returns>
        public Organism GetRandomOrganism()
        {
            return _species[Random.Next(_species.Count)].GetRandomOrganism(Random.Next);
        }

        /// <summary>
        /// Sets the organism's scores to what the client sends.
        /// </summary>
        /// <param name="organism">The organism.</param>
        /// <param name="score">The score.</param>
        public void PostScore(Organism organism, double score)
        {
            organism.Score = score;
        }

        /// <summary>
        /// Does 1 generation.
        /// kills the worst ones, mutate and breed and make the system ready for a new generation.
        /// </summary>
        public void EndGeneration()
        {
            HighestScore = double.MinValue;
            LowestScore = double.MaxValue;
            double totalFullScore = 0;

            foreach (Species species in _species)
            {
                foreach (Organism organism in species.Organisms)
                {
                    organism.Score /= _species.Count;
                    HighestScore = Math.Max(organism.Score, HighestScore);
                    LowestScore = Math.Min(organism.Score, LowestScore);
                    totalFullScore += organism.Score;
                }
            }

            AverageScore = totalFullScore / TrainingRoomSettings.OrganismCount;

            // Reproduce!
            foreach (Species species in _species)
            {
                species.PostGeneration(TrainingRoomSettings.TopAmountToSurvive);
            }

            double totalScore = _species.Sum(species => species.SpeciesScore);

            if ((int)totalScore == 0)
                totalScore = 1; // TODO: Think about what this really does lol, if the total score is 0 should they have the right to reproduce?

            double rest = 0;
            _organisms.Clear();
            foreach (Species species in _species)
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
                    _organisms.Add(ProduceOrganism(species));
                }
            }

            while (_organisms.Count < TrainingRoomSettings.OrganismCount)
            {
                _organisms.Add(new Organism(this));
            }

            foreach (Organism organism in _organisms)
            {
                AddOrganism(organism);
            }

            _species.RemoveAll(species => !species.Organisms.Any());

            _mutationToInnovation.Clear();
            Generation++;
        }

        /// <summary>
        /// Generates a organism based on the organisms from this species.
        /// A random organism is chosen and based on chance it can be bred with a random organism from this species or from the global pool.
        /// The organism is also mutated.
        /// </summary>
        /// <param name="species">The species.</param>
        /// <returns>Returns a generated <see cref="Organism"/>.</returns>
        private Organism ProduceOrganism(Species species)
        {
            Organism child = species.GetRandomOrganism(Random.Next);
            if (Random.NextDouble() < TrainingRoomSettings.CrossOverChance)
            {
                Organism parent2 = Random.NextDouble() < TrainingRoomSettings.InterSpeciesChance
                    ? GetRandomOrganism()
                    : species.GetRandomOrganism(Random.Next);

                child = child.Crossover(parent2);
            }
            else
                child = child.Clone();

            if (Random.NextDouble() < TrainingRoomSettings.MutationChance)
                child.Mutate();
            child.SpeciesId = Id;
            return child;
        }

        /// <summary>
        /// Increases the nodeID to the value passed in; If the nodeId is lower than the value passed in.
        /// </summary>
        /// <param name="min">The minimum value the nodeId should be.</param>
        public void IncreaseNodeIdTo(uint min)
        {
            _nodeId = Math.Max(_nodeId, min);
        }

        /// <summary>
        /// Gets the node id and increases it.
        /// </summary>
        /// <returns>Returns the old node id before increasing it.</returns>
        public uint GetAndIncreaseNodeId()
        {
            return _nodeId++;
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

            _mutationToInnovation.Add((inId, outId), ++InnovationId);
            return InnovationId;
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
            if (_authorizedUsers.Exists(usr => usr.Id.Equals(user.Id)))
                return false;
            _authorizedUsers.Add(user);
            return true;
        }

        /// <summary>
        /// Deauthorizes a user for the training room.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>Returns <c>true</c> if the user is removed from the authorized users; otherwise, <c>false</c>.</returns>
        public bool DeauthorizeUser(Guid userId)
        {
            User possibleUser = _authorizedUsers.SingleOrDefault(usr => usr.Id.Equals(userId));
            return possibleUser != default && _authorizedUsers.Remove(possibleUser);
        }

        /// <summary>
        /// Checks if the given user id is authorized.
        /// </summary>
        /// <param name="userId">The user id to verify.</param>
        /// <returns>Returns <c>true</c> if the given user id is authorized; otherwise, <c>false</c>.</returns>
        public bool IsUserAuthorized(Guid userId)
        {
            return _authorizedUsers.Exists(user => user.Id.Equals(userId));
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
            if (_trainingSessions.Any(user => user.Id.Equals(userId)))
                return false;
            _trainingSessions.Add(trainingSession);
            return true;
        }
    }
}
