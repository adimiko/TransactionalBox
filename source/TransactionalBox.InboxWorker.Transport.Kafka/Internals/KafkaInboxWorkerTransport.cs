using Confluent.Kafka;
using System.Runtime.CompilerServices;
using TransactionalBox.InboxWorker.Internals.Contexts;
using TransactionalBox.InboxWorker.Internals.Contracts;

namespace TransactionalBox.InboxWorker.Transport.Kafka.Internals
{
    internal sealed class KafkaInboxWorkerTransport : IInboxWorkerTransport
    {
        private readonly IInboxWorkerContext _inboxWorkerContext;

        private readonly KafkaConfigFactory _configFactory;

        public KafkaInboxWorkerTransport(
            IInboxWorkerContext inboxWorkerContext,
            KafkaConfigFactory configFactory) 
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
