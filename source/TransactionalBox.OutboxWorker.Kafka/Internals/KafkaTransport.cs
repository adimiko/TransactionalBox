using Confluent.Kafka;
using System.Text.Json;
using TransactionalBox.OutboxBase.StorageModel;
using TransactionalBox.OutboxWorker.Internals.Contracts;

namespace TransactionalBox.OutboxWorker.Kafka.Internals
{
    internal sealed class KafkaTransport : ITransport
    {
        private readonly KafkaConfigFactory _configFactory;

        public KafkaTransport(KafkaConfigFactory configFactory) 
        {
            _configFactory = configFactory;
        }

        public async Task AddRange(IEnumerable<OutboxMessage> messages)
        {
            var config = _configFactory.Create();

            using (var producer = new ProducerBuilder<Null, String>(config).Build())
            {
                //TODO #27
                var value = JsonSerializer.Serialize(messages);

                //TODO multiple topics
                //TODO order by Topic then order by OccurredUtc
                var result = await producer.ProduceAsync(messages.First().Topic, new Message<Null, string> { Value = value });

                //TODO throw exception
                //TODO (Processing) #26 
            }
        }
    }
}
