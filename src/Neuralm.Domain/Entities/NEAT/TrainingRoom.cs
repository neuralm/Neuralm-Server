using System;
using System.Collections.Generic;
using System.Linq;

namespace Neuralm.Domain.Entities.NEAT
{
    public class TrainingRoom
    {
        private readonly Dictionary<(uint A, uint B), uint> _mutationToInnovation;
        private readonly List<Species> _speciesList;
        private uint _nodeId;
        private readonly List<Brain> _children;
        private readonly List<User> _authorizedUsers;
        private readonly List<TrainingSession> _trainingSessions;

        public Guid Id { get; }
        public Guid OwnerId { get; }
        public User Owner { get; }
        public IReadOnlyList<User> AuthorizedUsers => _authorizedUsers;
        public IReadOnlyList<TrainingSession> TrainingSessions => _trainingSessions;
        public TrainingRoomSettings TrainingRoomSettings { get; }
        public string Name { get; }
        public uint Generation { get; private set; }

        public Random Random { get; }
        public double HighestScore { get; private set; }
        public double LowestScore { get; private set; }
        public double AverageScore { get; private set; }
        public uint InnovationId { get; private set; }
        public bool Enabled { get; private set; }

        /// <summary>
        /// Create a training room with the given settings.
        /// The training room manages all of the brains and the settings.
        /// </summary>
        /// <param name="owner">The user who created this training room.</param>
        /// <param name="name">The name for the room</param>
        /// <param name="trainingRoomSettings">The settings for this training room</param>
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
            _children = new List<Brain>((int)trainingRoomSettings.BrainCount);
            _mutationToInnovation = new Dictionary<(uint A, uint B), uint>();

            for (int i = 0; i < trainingRoomSettings.BrainCount; i++)
            {
                AddBrain(new Brain(TrainingRoomSettings.InputCount, TrainingRoomSettings.OutputCount, this));
            }
        }

        /// <summary>
        /// Add a brain to the training room
        /// Checks each species and creates a new species if no species matches.
        /// </summary>
        /// <param name="brain">The brain to add</param>
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
        /// Get a random brain from a random species
        /// </summary>
        /// <returns>A randomly chosen brain</returns>
        public Brain GetRandomBrain()
        {
            return _speciesList[Random.Next(_speciesList.Count)].GetRandomBrain();
        }

        /// <summary>
        /// Set the brain's scores to what the client send.
        /// </summary>
        /// <param name="brain"></param>
        /// <param name="score"></param>
        public void PostScore(Brain brain, double score)
        {
            brain.Score = score;
        }

        /// <summary>
        /// Do 1 generation.
        /// kill the worst ones, mutate and breed and make the system ready for a new generation.
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
            _children.Clear();
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
                    _children.Add(species.ProduceBrain());
                }
            }

            while (_children.Count < TrainingRoomSettings.BrainCount)
            {
                _children.Add(new Brain(TrainingRoomSettings.InputCount, TrainingRoomSettings.OutputCount, this));
            }

            foreach (Brain brain in _children)
            {
                AddBrain(brain);
            }

            _speciesList.RemoveAll(species => !species.Brains.Any());

            _mutationToInnovation.Clear();
            Generation++;
        }

        /// <summary>
        /// Increase to the nodeID to the value passed in if the nodeId is lower than the value passed in.
        /// </summary>
        /// <param name="max">The minimum value the nodeId should be</param>
        public void IncreaseNodeIdTo(uint max)
        {
            _nodeId = Math.Max(_nodeId, max);
        }

        /// <summary>
        /// Get the node id and increase it.
        /// </summary>
        /// <returns>The old node id before increasing it</returns>
        public uint GetAndIncreaseNodeId()
        {
            return _nodeId++;
        }

        /// <summary>
        /// Get the innovation number that corresponds with the given in and out node id.
        /// </summary>
        /// <param name="inId">The inId of the new connection</param>
        /// <param name="outId">The outId of the new connection</param>
        /// <returns>The innovation number</returns>
        public uint GetInnovationNumber(uint inId, uint outId)
        {
            if (_mutationToInnovation.ContainsKey((inId, outId)))
                return _mutationToInnovation[(inId, outId)];

            _mutationToInnovation.Add((inId, outId), ++InnovationId);
            return InnovationId;
        }

        /// <summary>
        /// Sets Enabled to false.
        /// </summary>
        public void Disable()
        {
            Enabled = false;
        }

        /// <summary>
        /// Sets Enabled to true.
        /// </summary>
        public void Enable()
        {
            // TODO: Is within re-enable period?
            Enabled = true;
        }

        /// <summary>
        /// Authorize a user for the training room.
        /// </summary>
        /// <param name="user">The user to authorize.</param>
        /// <returns>Returns true; if the user is added to the authorized users, else false.</returns>
        public bool AuthorizeUser(User user)
        {
            if (_authorizedUsers.Exists(usr => usr.Id.Equals(user.Id)))
                return false;
            _authorizedUsers.Add(user);
            return true;
        }

        /// <summary>
        /// Deauthorize a user for the training room.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>Returns true; if the user is removed from the authorized users, else false.</returns>
        public bool DeauthorizeUser(Guid userId)
        {
            User possibleUser = _authorizedUsers.SingleOrDefault(usr => usr.Id.Equals(userId));
            return possibleUser != default && _authorizedUsers.Remove(possibleUser);
        }

        /// <summary>
        /// Checks if the given user id is authorized.
        /// </summary>
        /// <param name="userId">The user id to verify.</param>
        /// <returns>Returns true; if the given user id is authorized, else false.</returns>
        public bool IsUserAuthorized(Guid userId)
        {
            return _authorizedUsers.Exists(user => user.Id.Equals(userId));
        }

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
