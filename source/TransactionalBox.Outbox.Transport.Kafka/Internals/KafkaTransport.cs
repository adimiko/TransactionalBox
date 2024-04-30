﻿using Confluent.Kafka;
using System.Text.Json;
using TransactionalBox.Base.Outbox.StorageModel;
using TransactionalBox.Outbox.Internals;
using TransactionalBox.Outbox.Internals.Contracts;

namespace TransactionalBox.Outbox.Transport.Kafka.Internals
{
    internal sealed class KafkaTransport : IOutboxWorkerTransport
    {
        private readonly KafkaConfigFactory _configFactory;

        public KafkaTransport(KafkaConfigFactory configFactory) 
        {
            _configFactory = configFactory;
        }

        public async Task<TransportResult> Add(string topic, byte[] payload)
        {
            var config = _configFactory.Create();

            using (var producer = new ProducerBuilder<Null, byte[]>(config).Build())
            {
                var result = await producer.ProduceAsync(topic, new Message<Null, byte[]> { Value = payload });

                if (result.Status != PersistenceStatus.Persisted)
                {
                    return TransportResult.Failure;
                }

                return TransportResult.Success;
            }
        }
    }
}