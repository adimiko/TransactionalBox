using Confluent.Kafka;
using System.Text.Json;
using TransactionalBox.Outbox.Internals;
using TransactionalBox.Outbox.Internals.Transport;

namespace TransactionalBox.Outbox.Transport.Kafka.Internals
{
    internal sealed class KafkaTransport : IOutboxWorkerTransport
    {
        private readonly KafkaConfigFactory _configFactory;

        public KafkaTransport(KafkaConfigFactory configFactory) 
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
