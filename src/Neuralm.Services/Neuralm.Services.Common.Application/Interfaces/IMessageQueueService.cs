using System.Threading;
using System.Threading.Tasks;

public interface IMessageQueueService 
{
    Task ListenAsync(CancellationToken cancellationToken);
}