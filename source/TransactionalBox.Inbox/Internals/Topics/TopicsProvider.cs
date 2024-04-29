using TransactionalBox.Inbox.Internals.Contexts;
using TransactionalBox.Inbox.Internals.Contracts;

namespace TransactionalBox.Inbox.Internals.Topics
{
    internal sealed class TopicsProvider : ITopicsProvider
    {
        public IEnumerable<string> Topics { get; }

        public TopicsProvider(
            IInboxWorkerContext inboxWorkerContext,
            ITransportTopicWithWildCard transportTopicWithWildCard)
        {
            var topics = new List<string>();

            var service = inboxWorkerContext.Id;

            var topicWithWildCard = transportTopicWithWildCard.GetTopicWithWildCard(service);
            //TODO topicWithWildCard remove
            //TODO create topics based on message types with attribute and without
            //TODO to base
            topics.Add(topicWithWildCard);

            Topics = topics;
        }
    }
}
