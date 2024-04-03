using Confluent.Kafka;
using TransactionalBox.InboxWorker.Internals.Contexts;
using TransactionalBox.Internals;

namespace TransactionalBox.InboxWorker.Transport.Kafka.Internals
{
    internal sealed class KafkaConfigFactory
    {
        private readonly IInboxWorkerContext _inboxWorkerContext;

        private readonly IInboxWorkerKafkaSettings _inboxWorkerKafkaSettings;

        private ConsumerConfig? _config = null;

        public KafkaConfigFactory(
            IInboxWorkerContext inboxWorkerContext,
            IInboxWorkerKafkaSettings inboxWorkerKafkaSettings) 
        {
            _inboxWorkerContext = inboxWorkerContext;
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
                GroupId = _inboxWorkerContext.Id,
                BootstrapServers = _inboxWorkerKafkaSettings.BootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false,
            };

            return _config;
        }
    }
}
