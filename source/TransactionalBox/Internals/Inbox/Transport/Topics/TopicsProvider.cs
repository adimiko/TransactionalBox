using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Internals.Inbox.Assemblies.MessageTypes;
using TransactionalBox.Internals.Inbox.InboxDefinitions;

namespace TransactionalBox.Internals.Inbox.Transport.Topics
{
    internal sealed class TopicsProvider : ITopicsProvider
    {
        public IEnumerable<string> Topics { get; }

        public TopicsProvider(
            IInboxMessageTypes inboxMessageTypes,
            IServiceContext serviceContext,
            ITopicFactory topicFactory,
            IServiceProvider serviceProvider)
        {
            var topics = new List<string>();

            var messageTypes = inboxMessageTypes.MessageTypes;

            var inboxMessageDefinitionDictionary = new Dictionary<Type, IInboxDefinition>();

            var defaultInboxMessageDefinition = new DefaultInboxDefinition();

            foreach (var messageType in messageTypes)
            {
                var x = serviceProvider.GetKeyedServices<IInboxDefinition>(messageType);

                //TODO custom Exception
                var inboxMessageDefinition = x.SingleOrDefault();

                if (inboxMessageDefinition is null)
                {
                    inboxMessageDefinitionDictionary.Add(messageType, defaultInboxMessageDefinition);
                }
                else
                {
                    inboxMessageDefinitionDictionary.Add(messageType, inboxMessageDefinition);
                }
            }

            foreach (var x in inboxMessageDefinitionDictionary)
            {
                string topic;

                if (x.Value.PublishedBy is null)
                {
                    topic = topicFactory.Create(serviceContext.Id, x.Key.Name);
                }
                else
                {
                    topic = topicFactory.Create(x.Value.PublishedBy, x.Key.Name);
                }

                topics.Add(topic);
            }

            Topics = topics;
        }
    }
}
