using Microsoft.VisualBasic;
using System.Data;
using TransactionalBox.Inbox.Internals.Contexts;
using TransactionalBox.Inbox.Internals.Contracts;

namespace TransactionalBox.Inbox.Internals.Topics
{
    internal sealed class TopicsProvider : ITopicsProvider
    {
        private static readonly Type _attributeType = typeof(PublishedByAttribute);

        public IEnumerable<string> Topics { get; }

        public TopicsProvider(
            IInboxWorkerContext inboxWorkerContext,
            IInboxMessageTypes inboxMessageTypes,
            ITransportTopicWithWildCard transportTopicWithWildCard)
        {
            var topics = new List<string>();

            var service = inboxWorkerContext.Id;

            var messageTypes = inboxMessageTypes.MessageTypes;

            var publishedMessageTypes = messageTypes.Where(x => Attribute.IsDefined(x, _attributeType));
            var sentMessageTypes = messageTypes.Where(x => !Attribute.IsDefined(x, _attributeType));

            if (sentMessageTypes.Any()) 
            {
                //TODO when create without wildcard
                var topicWithWildCard = transportTopicWithWildCard.GetTopicWithWildCard(service);
                topics.Add(topicWithWildCard);
            }

            foreach(var publishedMessageType in  publishedMessageTypes) 
            {
                PublishedByAttribute publishedByAttributes = (PublishedByAttribute) Attribute.GetCustomAttribute(publishedMessageType, _attributeType);

                var publishedByName = publishedByAttributes.PublishedBy;
                var messageName = publishedMessageType.Name;

                //TODO TopicFactory
                topics.Add(publishedByName + '.' + messageName);
            }

            Topics = topics;
        }
    }
}
