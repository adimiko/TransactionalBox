using System.Data;
using TransactionalBox.Inbox.Internals.Assemblies.MessageTypes;
using TransactionalBox.Inbox.Internals.Contexts;
using TransactionalBox.Internals;

namespace TransactionalBox.Inbox.Internals.Transport.Topics
{
    internal sealed class TopicsProvider : ITopicsProvider
    {
        private static readonly Type _attributeType = typeof(PublishedByAttribute);

        public IEnumerable<string> Topics { get; }

        public TopicsProvider(
            IInboxContext inboxWorkerContext, //TODO remove ?
            IInboxMessageTypes inboxMessageTypes,
            IServiceContext serviceContext,
            ITopicFactory topicFactory)
        {
            var topics = new List<string>();

            var messageTypes = inboxMessageTypes.MessageTypes;

            var publishedMessageTypes = messageTypes.Where(x => Attribute.IsDefined(x, _attributeType));
            var sentMessageTypes = messageTypes.Where(x => !Attribute.IsDefined(x, _attributeType));

            foreach(var sentMessageType in sentMessageTypes)
            {
                var topic = topicFactory.Create(serviceContext.Id, sentMessageType.Name);

                topics.Add(topic);
            }

            foreach (var publishedMessageType in publishedMessageTypes)
            {
                PublishedByAttribute publishedByAttributes = (PublishedByAttribute)Attribute.GetCustomAttribute(publishedMessageType, _attributeType);

                var topic = topicFactory.Create(publishedByAttributes.PublishedBy, publishedMessageType.Name);

                topics.Add(topic);
            }

            Topics = topics;
        }
    }
}
