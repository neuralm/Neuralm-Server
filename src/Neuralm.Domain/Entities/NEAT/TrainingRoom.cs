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
        private readonly List<Species> _speciesList;
        private uint _nodeId;
        private List<Brain> _brains;
        private List<User> _authorizedUsers;
        private List<TrainingSession> _trainingSessions;

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
        /// Gets the list of brains.
        /// </summary>
        public virtual IReadOnlyList<Brain> Brains => _brains;

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
        /// The training room manages all of the brains and the settings.
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
            _speciesList = new List<Species>((int)trainingRoomSettings.BrainCount);
            _brains = new List<Brain>((int)trainingRoomSettings.BrainCount);
            _mutationToInnovation = new Dictionary<(uint A, uint B), uint>();

            for (int i = 0; i < trainingRoomSettings.BrainCount; i++)
            {
                AddBrain(new Brain(TrainingRoomSettings.InputCount, TrainingRoomSettings.OutputCount, this));
            }
        }

        /// <summary>
        /// Adds a brain to the training room.
        /// Checks each species and creates a new species if no species matches.
        /// </summary>
        /// <param name="brain">The brain to add.</param>
        public void AddBrain(Brain brain)
        {
            foreach (Species species in _speciesList)
            {
                if (species.AddBrainIfSameSpecies(brain))
                {
                    return;
                }
            }

            _speciesList.Add(new Species(brain, this));
        }

        /// <summary>
        /// Gets a random brain from a random species.
        /// </summary>
        /// <returns>A randomly chosen <see cref="Brain"/>.</returns>
        public Brain GetRandomBrain()
        {
            return _speciesList[Random.Next(_speciesList.Count)].GetRandomBrain();
        }

        /// <summary>
        /// Sets the brain's scores to what the client sends.
        /// </summary>
        /// <param name="brain">The brain.</param>
        /// <param name="score">The score.</param>
        public void PostScore(Brain brain, double score)
        {
            brain.Score = score;
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

            foreach (Species species in _speciesList)
            {
                foreach (Brain brain in species.Brains)
                {
                    brain.Score /= _speciesList.Count;
                    HighestScore = Math.Max(brain.Score, HighestScore);
                    LowestScore = Math.Min(brain.Score, LowestScore);
                    totalFullScore += brain.Score;
                }
            }

            AverageScore = totalFullScore / TrainingRoomSettings.BrainCount;

            // Reproduce!
            foreach (Species species in _speciesList)
            {
                species.PostGeneration();
            }

            double totalScore = _speciesList.Sum(species => species.SpeciesScore);

            if ((int)totalScore == 0)
                totalScore = 1; // TODO: Think about what this really does lol, if the total score is 0 should they have the right to reproduce?

            double rest = 0;
            _brains.Clear();
            foreach (Species species in _speciesList)
            {
                double fraction = species.SpeciesScore / totalScore;
                double amountOfBrains = TrainingRoomSettings.BrainCount * fraction;
                rest += amountOfBrains % 1;

                if (rest >= 1)
                {
                    amountOfBrains++;
                    rest--;
                }

                amountOfBrains = Math.Floor(amountOfBrains);
                for (int i = 0; i < amountOfBrains; i++)
                {
                    _brains.Add(species.ProduceBrain());
                }
            }

            while (_brains.Count < TrainingRoomSettings.BrainCount)
            {
                _brains.Add(new Brain(TrainingRoomSettings.InputCount, TrainingRoomSettings.OutputCount, this));
            }

            foreach (Brain brain in _brains)
            {
                AddBrain(brain);
            }

            _speciesList.RemoveAll(species => !species.Brains.Any());

            _mutationToInnovation.Clear();
            Generation++;
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
