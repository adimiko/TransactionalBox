using Confluent.Kafka;
using System.Text.Json;
using TransactionalBox.OutboxBase.StorageModel;
using TransactionalBox.OutboxWorker.Internals;

namespace TransactionalBox.OutboxWorker.Kafka.Internals
{
    internal sealed class KafkaTransport : ITransport
    {
        private readonly KafkaConfigFactory _configFactory;

        public KafkaTransport(KafkaConfigFactory configFactory) 
        {
            _configFactory = configFactory;
        }

        public async Task Add(OutboxMessage message)
        {
            var config = _configFactory.Create();

            using (var producer = new ProducerBuilder<Null, String>(config).Build())
            {
                //TODO #27
                var value = JsonSerializer.Serialize(message);

                var result = await producer.ProduceAsync(message.Topic, new Message<Null, string> { Value = value });

                //TODO throw exception
                //TODO (Processing) #26 
            }
        }
    }
}
