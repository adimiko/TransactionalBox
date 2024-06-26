﻿using Confluent.Kafka;
using System.Runtime.CompilerServices;
using System.Text;
using TransactionalBox.Internals.Inbox.Transport;
using TransactionalBox.Internals.Inbox.Transport.ContractsToImplement;

namespace TransactionalBox.Kafka.Internals.Inbox.ImplementedContracts
{
    internal sealed class KafkaInboxTransport : IInboxTransport
    {
        private readonly KafkaConsumerConfigFactory _configFactory;

        public KafkaInboxTransport(KafkaConsumerConfigFactory configFactory)
        {
            _configFactory = configFactory;
        }

        public async IAsyncEnumerable<TransportMessage> GetMessages(IEnumerable<string> topics, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var config = _configFactory.Create();

            using (var consumer = new ConsumerBuilder<Ignore, byte[]>(config).Build())
            {
                consumer.Subscribe(topics);

                do
                {
                    var result = consumer.Consume();

                    //TODO valid
                    var x = result.Headers.Single(x => x.Key == "Compression");

                    var transportMessage = new TransportMessage()
                    {
                        Payload = result.Message.Value,
                        Compression = Encoding.UTF8.GetString(x.GetValueBytes()),
                    };

                    yield return transportMessage;

                    consumer.Commit(result);
                }
                while (!cancellationToken.IsCancellationRequested);

                consumer.Close();
            }

            await Task.CompletedTask;
        }
    }
}
