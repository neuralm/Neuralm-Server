namespace Neuralm.Application.Messages.Events
{
    /// <summary>
    /// Represents the <see cref="MonitorEvent"/> class.
    /// </summary>
    public sealed class MonitorEvent : Event
    {
        /// <summary>
        /// Gets the status.
        /// </summary>
        public string Status { get; }

        /// <summary>
        /// Initializes an instance of the <see cref="MonitorEvent"/> class.
        /// </summary>
        /// <param name="status">The status.</param>
        public MonitorEvent(string status)
        {
            Status = status;
        }
    }
}
