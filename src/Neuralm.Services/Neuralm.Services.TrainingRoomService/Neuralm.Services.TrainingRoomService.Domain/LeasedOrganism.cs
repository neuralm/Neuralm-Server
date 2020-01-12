using Neuralm.Services.Common.Domain;
using System;

namespace Neuralm.Services.TrainingRoomService.Domain
{
    /// <summary>
    /// Represents the <see cref="LeasedOrganism"/> class.
    /// </summary>
    public class LeasedOrganism : IEntity
    {
        /// <summary>
        /// Gets and sets the id.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets and sets the organism id.
        /// </summary>
        public Guid? OrganismId { get; private set; }

        /// <summary>
        /// Gets and sets the organism.
        /// </summary>
        public virtual Organism Organism { get; set; }

        /// <summary>
        /// Gets and sets the lease start date time.
        /// </summary>
        public DateTime LeaseStart { get; private set; }

        /// <summary>
        /// Gets and sets the lease end date time.
        /// </summary>
        public DateTime LeaseEnd { get; private set; }
        
        /// <summary>
        /// Gets and sets the training session id.
        /// </summary>
        public Guid TrainingSessionId { get; private set; }

        /// <summary>
        /// EFCore Entity constructor IGNORE!
        /// </summary>
        protected LeasedOrganism()
        {

        }

        /// <summary>
        /// Initializes an instance of the <see cref="LeasedOrganism"/> class.
        /// </summary>
        /// <param name="organism">The organism.</param>
        /// <param name="trainingSessionId">The training session id.</param>
        public LeasedOrganism(Organism organism, Guid trainingSessionId)
        {
            Id = Guid.NewGuid();
            Organism = organism;
            TrainingSessionId = trainingSessionId;
            OrganismId = organism.Id;
            LeaseStart = DateTime.UtcNow;
            LeaseEnd = DateTime.UtcNow.AddHours(2);
        }

        public override string ToString()
        {
            return $"Id: {Id}, OrganismId: {OrganismId}, TrainingSessionId: {TrainingSessionId}, LeaseStart: {LeaseStart}, LeaseEnd: {LeaseEnd}";
        }
    }
}
