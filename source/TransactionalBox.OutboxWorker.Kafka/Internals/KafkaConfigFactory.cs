using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TransactionalBox.OutboxWorker.Kafka.Internals
{
    internal sealed class KafkaConfigFactory
    {
        private readonly IOutboxWorkerKafkaSettings _outboxWorkerKafkaSettings;

        private ProducerConfig? _config = null;

        public KafkaConfigFactory(IOutboxWorkerKafkaSettings outboxWorkerKafkaSettings) 
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
                ClientId = Dns.GetHostName(), // TODO (HostNameService) #25
            };

            return _config;
        }
    }
}
