using Confluent.Kafka.Admin;
using Confluent.Kafka;
using TransactionalBox.Inbox.Internals.Transport.Topics;

namespace TransactionalBox.Inbox.Kafka.Internals
{
    internal sealed class KafkaTransportTopicsCreator : ITransportTopicsCreator
    {
        private readonly IInboxKafkaSettings _settings;

        public KafkaTransportTopicsCreator(IInboxKafkaSettings settings) 
        {
            _settings = settings;
        }

        public async Task Create(IEnumerable<string> topics)
        {
            var topicSpecifications = new List<TopicSpecification>();

            foreach (var topic in topics) 
            {
                topicSpecifications.Add(new TopicSpecification() { Name =  topic });
            }

            using (var client = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = _settings.BootstrapServers }).Build())
            {
                try
                {
                    //TODO only one (if exist don't create)
                    await client.CreateTopicsAsync(topicSpecifications).ConfigureAwait(false);
                }
                catch (CreateTopicsException)
                {
                    
                }
            };
        }
    }
}
