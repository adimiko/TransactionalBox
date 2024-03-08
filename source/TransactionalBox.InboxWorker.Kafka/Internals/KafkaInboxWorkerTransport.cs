using Confluent.Kafka;
using System.Runtime.CompilerServices;
using System.Text.Json;
using TransactionalBox.InboxBase.StorageModel;
using TransactionalBox.InboxWorker.Internals;
using TransactionalBox.Internals;

namespace TransactionalBox.InboxWorker.Kafka.Internals
{
    internal sealed class KafkaInboxWorkerTransport : IInboxWorkerTransport
    {
        private readonly ITransactionalBoxSettings _transactionalBoxSettings;

        private readonly KafkaConfigFactory _configFactory;

        public KafkaInboxWorkerTransport(
            ITransactionalBoxSettings transactionalBoxSettings,
            KafkaConfigFactory configFactory) 
        {
            _transactionalBoxSettings = transactionalBoxSettings;
            _configFactory = configFactory;
        }

        public async IAsyncEnumerable<InboxMessage> GetMessage([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var config = _configFactory.Create();

            using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                //TODO #41
                consumer.Subscribe($"{_transactionalBoxSettings.ServiceName}-ExampleMessage");

                do
                {
                    var result = consumer.Consume();

                    //TODO #27
                    var message = JsonSerializer.Deserialize<InboxMessage>(result.Message.Value);

                    yield return message;

                    consumer.Commit(result);
                }
                while (!cancellationToken.IsCancellationRequested);

                consumer.Close();
            }

            await Task.CompletedTask;
        }
    }
}
