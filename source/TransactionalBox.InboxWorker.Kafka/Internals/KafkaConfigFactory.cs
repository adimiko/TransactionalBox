using Confluent.Kafka;
using TransactionalBox.Internals;

namespace TransactionalBox.InboxWorker.Kafka.Internals
{
    internal sealed class KafkaConfigFactory
    {
        private readonly ITransactionalBoxSettings _transactionalBoxSettings;

        private readonly IInboxWorkerKafkaSettings _inboxWorkerKafkaSettings;

        private ConsumerConfig? _config = null;

        public KafkaConfigFactory(
            ITransactionalBoxSettings transactionalBoxSettings,
            IInboxWorkerKafkaSettings inboxWorkerKafkaSettings) 
        {
            _transactionalBoxSettings = transactionalBoxSettings;
            _inboxWorkerKafkaSettings = inboxWorkerKafkaSettings;
        }

        internal ConsumerConfig Create()
        {
            if (_config is not null) 
            {
                return _config;
            }

            _config = new ConsumerConfig()
            {
                GroupId = _transactionalBoxSettings.ServiceName,
                BootstrapServers = _inboxWorkerKafkaSettings.BootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false,
            };

            return _config;
        }
    }
}
