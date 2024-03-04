using Confluent.Kafka;
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

        public async IAsyncEnumerable<string> GetMessage(CancellationToken cancellationToken)
        {
            using (var consumer = new ConsumerBuilder<Ignore, string>(_config).Build())
            {
                consumer.Subscribe("ModuleName-ExampleMessage");

                do
                {
                    var result = consumer.Consume();
                    
                    var message = result.Message.Value;

                    yield return message;

                    consumer.Commit(result);
                }
                while (!cancellationToken.IsCancellationRequested);

                consumer.Close();
            }
        }
    }
}
