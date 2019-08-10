using System;

namespace Neuralm.Domain.Entities.NEAT
{
    /// <summary>
    /// Represents the <see cref="LeasedOrganism"/> class.
    /// </summary>
    public class LeasedOrganism
    {
        /// <summary>
        /// Gets and sets the id.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets and sets the organism id.
        /// </summary>
        public Guid OrganismId { get; private set; }

        /// <summary>
        /// Gets and sets the organism.
        /// </summary>
        public Organism Organism { get; set; }

        /// <summary>
        /// Gets and sets the lease start date time.
        /// </summary>
        public DateTime LeaseStart { get; private set; }

        /// <summary>
        /// Gets and sets the lease end date time.
        /// </summary>
        public DateTime LeaseEnd { get; private set; }

        /// <summary>
        /// EFCore Entity constructor IGNORE!
        /// </summary>
        private LeasedOrganism()
        {
            
        }

        /// <summary>
        /// Initializes an instance of the <see cref="LeasedOrganism"/> class.
        /// </summary>
        /// <param name="organism">The organism.</param>
        public LeasedOrganism(Organism organism)
        {
            Id = Guid.NewGuid();
            Organism = organism;
            OrganismId = organism.Id;
            LeaseStart = DateTime.UtcNow;
            LeaseEnd = DateTime.UtcNow.AddHours(2);
        }
    }
}
