using Confluent.Kafka;

namespace TransactionalBox.InboxWorker.Kafka.Internals
{
    internal sealed class KafkaConfigFactory
    {
        private readonly IServiceContext _serviceContext;

        private readonly IInboxWorkerKafkaSettings _inboxWorkerKafkaSettings;

        private ConsumerConfig? _config = null;

        public KafkaConfigFactory(
            IServiceContext serviceContext,
            IInboxWorkerKafkaSettings inboxWorkerKafkaSettings) 
        {
            _serviceContext = serviceContext;
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
                GroupId = _serviceContext.Id,
                BootstrapServers = _inboxWorkerKafkaSettings.BootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false,
            };

            return _config;
        }
    }
}
