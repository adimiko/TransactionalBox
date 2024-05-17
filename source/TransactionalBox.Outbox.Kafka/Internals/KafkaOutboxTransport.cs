using Confluent.Kafka;
using TransactionalBox.Outbox.Internals.Transport;

namespace TransactionalBox.Outbox.Kafka.Internals
{
    internal sealed class KafkaOutboxTransport : IOutboxTransport
    {
        private readonly KafkaConfigFactory _configFactory;

        public KafkaOutboxTransport(KafkaConfigFactory configFactory) 
        {
            _configFactory = configFactory;
        }

        public async Task Add(string topic, byte[] payload)
        {
            var config = _configFactory.Create();

            //TODO create one producer and hold connection ?
            using (var producer = new ProducerBuilder<Null, byte[]>(config).Build())
            {
                var result = await producer.ProduceAsync(topic, new Message<Null, byte[]> { Value = payload });

                if (result.Status != PersistenceStatus.Persisted)
                {
                    throw new FailedAddMessagesToTransportException();
                }
            }
        }
    }
}
