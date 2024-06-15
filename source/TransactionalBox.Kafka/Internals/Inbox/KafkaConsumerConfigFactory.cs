using Confluent.Kafka;
using TransactionalBox.Internals;

namespace TransactionalBox.Kafka.Internals.Inbox
{
    internal sealed class KafkaConsumerConfigFactory
    {
        private readonly IServiceContext _serviceContext;

        private readonly IInboxKafkaSettings _inboxWorkerKafkaSettings;

        private ConsumerConfig? _config = null;

        public KafkaConsumerConfigFactory(
            IServiceContext serviceContext,
            IInboxKafkaSettings inboxWorkerKafkaSettings)
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
                ClientId = _serviceContext.Id + Guid.NewGuid().ToString(), //TODO
                GroupId = _serviceContext.Id,
                BootstrapServers = _inboxWorkerKafkaSettings.BootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false,
            };

            return _config;
        }
    }
}
