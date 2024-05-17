using Confluent.Kafka;
using System.Runtime.CompilerServices;
using TransactionalBox.Inbox.Internals.Contexts;
using TransactionalBox.Inbox.Internals.Transport;

namespace TransactionalBox.Inbox.Kafka.Internals
{
    internal sealed class KafkaInboxTransport : IInboxTransport
    {
        private readonly IInboxContext _inboxWorkerContext;

        private readonly KafkaConsumerConfigFactory _configFactory;

        public KafkaInboxTransport(
            IInboxContext inboxWorkerContext,
            KafkaConsumerConfigFactory configFactory) 
        {
            _inboxWorkerContext = inboxWorkerContext;
            _configFactory = configFactory;
        }

        public async IAsyncEnumerable<byte[]> GetMessages(IEnumerable<string> topics, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var config = _configFactory.Create();

            using (var consumer = new ConsumerBuilder<Ignore, byte[]>(config).Build())
            {
                consumer.Subscribe(topics);

                do
                {
                    var result = consumer.Consume();

                    yield return result.Message.Value;

                    consumer.Commit(result);
                }
                while (!cancellationToken.IsCancellationRequested);

                consumer.Close();
            }

            await Task.CompletedTask;
        }
    }
}
