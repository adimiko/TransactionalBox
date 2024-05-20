using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Inbox.Internals.Assemblies.MessageTypes;
using TransactionalBox.Inbox.Internals.InboxMessageDefinitions;
using TransactionalBox.Internals;

namespace TransactionalBox.Inbox.Internals.Transport.Topics
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

            var inboxMessageDefinitionDictionary = new Dictionary<Type, IInboxMessageDefinition>();

            var defaultInboxMessageDefinition = new DefaultInboxMessageDefinition();

            foreach ( var messageType in messageTypes ) 
            {
                var x = serviceProvider.GetKeyedServices<IInboxMessageDefinition>(messageType);

                //TODO custom Exception
                var inboxMessageDefinition = x.SingleOrDefault();

                if(inboxMessageDefinition is null)
                {
                    inboxMessageDefinitionDictionary.Add(messageType, defaultInboxMessageDefinition);
                }
                else
                {
                    inboxMessageDefinitionDictionary.Add(messageType, inboxMessageDefinition);
                }
            }

            foreach(var x in inboxMessageDefinitionDictionary) 
            {
                string topic;

                if(x.Value.PublishedBy is null)
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
