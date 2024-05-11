using Confluent.Kafka;

namespace TransactionalBox.Outbox.Transport.Kafka.Internals
{
    internal sealed class KafkaConfigFactory
    {
        private readonly IOutboxKafkaSettings _outboxWorkerKafkaSettings;

        private ProducerConfig? _config = null;

        public KafkaConfigFactory(
            IOutboxKafkaSettings outboxWorkerKafkaSettings) 
        {
            _outboxWorkerKafkaSettings = outboxWorkerKafkaSettings;
        }

        internal ProducerConfig Create()
        {
            if (_config is not null) 
            {
                return _config;
            }

            _config = new ProducerConfig()
            {
                BootstrapServers = _outboxWorkerKafkaSettings.BootstrapServers,
            };

            return _config;
        }
    }
}
