using TransactionalBox.InboxWorker.Internals.Contexts;
using TransactionalBox.InboxWorker.Internals.Contracts;

namespace TransactionalBox.InboxWorker.Internals.Services
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
