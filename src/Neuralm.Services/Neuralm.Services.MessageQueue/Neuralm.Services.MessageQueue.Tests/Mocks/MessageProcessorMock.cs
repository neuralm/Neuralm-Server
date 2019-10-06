using Neuralm.Services.Common.Messages.Interfaces;
using Neuralm.Services.MessageQueue.Application.Interfaces;
using Neuralm.Services.MessageQueue.Domain;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Neuralm.Services.MessageQueue.Tests.Mocks
{
    public class MessageProcessorMock : IMessageProcessor
    {
        private readonly IMessageSerializer _messageSerializer;
        private readonly AsyncConcurrentQueue<IMessage> _messages;

        public MessageProcessorMock(IMessageSerializer messageSerializer)
        {
            _messageSerializer = messageSerializer;
            _messages = new AsyncConcurrentQueue<IMessage>();
        }

        public Task ProcessMessageAsync(IMessage message, INetworkConnector networkConnector)
        {
            _messages.Enqueue(message);
            Console.WriteLine(Encoding.UTF8.GetString(_messageSerializer.Serialize(message).ToArray()));
            return Task.CompletedTask;
        }

        public Task<IMessage> GetMessageAsync(CancellationToken cancellationToken)
        {
            return _messages.DequeueAsync(cancellationToken);
        }
    }
}
