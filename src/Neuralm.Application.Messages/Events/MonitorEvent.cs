namespace Neuralm.Application.Messages.Events
{
    public sealed class MonitorEvent : Event
    {
        public string Status { get; }

        public MonitorEvent(string status)
        {
            Status = status;
        }
    }
}
