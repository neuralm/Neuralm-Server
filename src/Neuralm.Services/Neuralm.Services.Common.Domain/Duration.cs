namespace Neuralm.Services.Common.Domain
{
    /// <summary>
    /// Represents the <see cref="Duration"/> class.
    /// Used to represent a timespan with only the ticks, seconds, and milliseconds components.
    /// </summary>
    public class Duration
    {
        /// <summary>
        /// Gets and sets the ticks.
        /// </summary>
        public long Ticks { get; set; }

        /// <summary>
        /// Gets and sets the seconds.
        /// </summary>
        public int Seconds { get; set; }

        /// <summary>
        /// Gets and sets the milliseconds.
        /// </summary>
        public int Milliseconds { get; set; }
    }
}
