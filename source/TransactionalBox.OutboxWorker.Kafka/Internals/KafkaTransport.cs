using Confluent.Kafka;
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

        public async Task Add(string message, string topic)
        {
            using (var producer = new ProducerBuilder<Null, String>(_config).Build())
            {
                var result = await producer.ProduceAsync(topic, new Message<Null, string> { Value = message });

                //TODO (Processing) #26 
            }
        }
    }
}
