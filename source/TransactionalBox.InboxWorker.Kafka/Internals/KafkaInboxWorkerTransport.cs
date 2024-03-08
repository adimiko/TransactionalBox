using Confluent.Kafka;
using System.Runtime.CompilerServices;
using System.Text.Json;
using TransactionalBox.InboxBase.StorageModel;
using TransactionalBox.InboxWorker.Internals;

namespace TransactionalBox.InboxWorker.Kafka.Internals
{
    internal sealed class KafkaInboxWorkerTransport : IInboxWorkerTransport
    {
        private readonly ConsumerConfig _config;

        public KafkaInboxWorkerTransport(ConsumerConfig config) 
        {
            _config = config;
        }

        public async IAsyncEnumerable<InboxMessage> GetMessage([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            using (var consumer = new ConsumerBuilder<Ignore, string>(_config).Build())
            {
                //TODO #28
                consumer.Subscribe("ModuleName-ExampleMessage");

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
