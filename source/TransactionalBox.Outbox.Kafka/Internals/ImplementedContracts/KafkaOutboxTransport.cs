using Confluent.Kafka;
using System.Text;
using TransactionalBox.Outbox.Internals.Hooks.Handlers.AddMessagesToTransport.TransportMessageFactories;
using TransactionalBox.Outbox.Internals.Transport;
using TransactionalBox.Outbox.Internals.Transport.ContractsToImplement;

namespace TransactionalBox.Outbox.Kafka.Internals.ImplementedContracts
{
    internal sealed class KafkaOutboxTransport : IOutboxTransport
    {
        private readonly KafkaConfigFactory _configFactory;

        public KafkaOutboxTransport(KafkaConfigFactory configFactory)
        {
            _configFactory = configFactory;
        }

        public async Task Add(TransportEnvelope transportEnvelope)
        {
            var config = _configFactory.Create();

            var headers = new Headers();

            headers.Add("Compression", Encoding.UTF8.GetBytes(transportEnvelope.Compression));

            //TODO create one producer and hold connection ?
            using (var producer = new ProducerBuilder<Null, byte[]>(config).Build())
            {
                var result = await producer.ProduceAsync(transportEnvelope.Topic, new Message<Null, byte[]> { Value = transportEnvelope.Payload, Headers = headers });

                if (result.Status != PersistenceStatus.Persisted)
                {
                    throw new FailedAddMessagesToTransportException();
                }
            }
        }
    }
}
