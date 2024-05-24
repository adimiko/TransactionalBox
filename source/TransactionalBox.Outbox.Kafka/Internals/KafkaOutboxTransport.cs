using Confluent.Kafka;
using System.Text;
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

        public async Task Add(string topic, byte[] payload, string contentType)
        {
            var config = _configFactory.Create();

            var headers = new Headers();

            headers.Add("ContentType", Encoding.UTF8.GetBytes(contentType));

            //TODO create one producer and hold connection ?
            using (var producer = new ProducerBuilder<Null, byte[]>(config).Build())
            {
                var result = await producer.ProduceAsync(topic, new Message<Null, byte[]> { Value = payload, Headers = headers });

                if (result.Status != PersistenceStatus.Persisted)
                {
                    throw new FailedAddMessagesToTransportException();
                }
            }
        }
    }
}
