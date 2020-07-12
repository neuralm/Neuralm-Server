namespace Neuralm.Services.Common.Messages.Dtos
{
    /// <summary>
    /// Represents the <see cref="DurationDto"/> of the Duration class.
    /// </summary>
    public class DurationDto
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
