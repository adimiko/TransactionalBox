using Confluent.Kafka;
using System.Runtime.CompilerServices;
using System.Text.Json;
using TransactionalBox.InboxBase.StorageModel.Internals;
using TransactionalBox.InboxWorker.Internals.Contracts;

namespace TransactionalBox.InboxWorker.Kafka.Internals
{
    internal sealed class KafkaInboxWorkerTransport : IInboxWorkerTransport
    {
        private readonly IServiceContext _serviceContext;

        private readonly KafkaConfigFactory _configFactory;

        public KafkaInboxWorkerTransport(
            IServiceContext serviceContext,
            KafkaConfigFactory configFactory) 
        {
            _serviceContext = serviceContext;
            _configFactory = configFactory;
        }

        public async IAsyncEnumerable<IEnumerable<InboxMessage>> GetMessages([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var config = _configFactory.Create();

            using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                //TODO #41
                consumer.Subscribe($"{_serviceContext.Id}-ExampleMessage");

                do
                {
                    var result = consumer.Consume();

                    //TODO #27
                    var messages = JsonSerializer.Deserialize<IEnumerable<InboxMessage>>(result.Message.Value);

                    yield return messages;

                    consumer.Commit(result);
                }
                while (!cancellationToken.IsCancellationRequested);

                consumer.Close();
            }

            await Task.CompletedTask;
        }
    }
}
