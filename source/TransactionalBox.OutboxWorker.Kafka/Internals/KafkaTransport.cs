using Confluent.Kafka;
using System.Text.Json;
using TransactionalBox.OutboxBase.StorageModel;
using TransactionalBox.OutboxWorker.Internals;
using TransactionalBox.OutboxWorker.Internals.Contracts;

namespace TransactionalBox.OutboxWorker.Kafka.Internals
{
    internal sealed class KafkaTransport : ITransport
    {
        private readonly KafkaConfigFactory _configFactory;

        public KafkaTransport(KafkaConfigFactory configFactory) 
        {
            _configFactory = configFactory;
        }

        public async Task<TransportResult> Add(IEnumerable<TransportMessage> transportMessages)
        {
            var config = _configFactory.Create();

            using (var producer = new ProducerBuilder<Null, String>(config).Build())
            {
                foreach (var transportMessage in transportMessages) 
                {
                    var result = await producer.ProduceAsync(transportMessage.Topic, new Message<Null, string> { Value = transportMessage.Payload });
                    
                    if (result.Status != PersistenceStatus.Persisted)
                    {
                        return TransportResult.Failure;
                    }
                }

                return TransportResult.Success;
            }
        }
    }
}
