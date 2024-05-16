using Confluent.Kafka;
using TransactionalBox.Inbox.Internals.Contexts;

namespace TransactionalBox.Inbox.Transport.Kafka.Internals
{
    internal sealed class KafkaConsumerConfigFactory
    {
        private readonly IInboxContext _inboxWorkerContext;

        private readonly IInboxKafkaSettings _inboxWorkerKafkaSettings;

        private ConsumerConfig? _config = null;

        public KafkaConsumerConfigFactory(
            IInboxContext inboxWorkerContext,
            IInboxKafkaSettings inboxWorkerKafkaSettings) 
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
                ClientId = _inboxWorkerContext.Id + Guid.NewGuid().ToString(), //TODO
                GroupId = _inboxWorkerContext.Id,
                BootstrapServers = _inboxWorkerKafkaSettings.BootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false,
            };

            return _config;
        }
    }
}
