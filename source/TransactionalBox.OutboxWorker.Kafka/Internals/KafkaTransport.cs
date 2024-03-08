using Confluent.Kafka;
using System.Text.Json;
using TransactionalBox.OutboxBase.StorageModel;
using TransactionalBox.OutboxWorker.Internals;

namespace TransactionalBox.OutboxWorker.Kafka.Internals
{
    internal sealed class KafkaTransport : ITransport
    {
        private readonly ProducerConfig _config;

        public KafkaTransport(ProducerConfig config) 
        {
            _config = config;
        }

        public async Task Add(OutboxMessage message)
        {
            using (var producer = new ProducerBuilder<Null, String>(_config).Build())
            {
                //TODO #27
                var value = JsonSerializer.Serialize(message);

                var result = await producer.ProduceAsync(message.Topic, new Message<Null, string> { Value = value });

                //TODO (Processing) #26 
            }
        }
    }
}
