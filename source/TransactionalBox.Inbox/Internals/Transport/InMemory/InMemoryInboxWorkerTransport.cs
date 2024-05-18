using TransactionalBox.Internals.Transport.InMemory.Internals;

namespace TransactionalBox.Inbox.Internals.Transport.InMemory
{
    internal sealed class InMemoryInboxWorkerTransport : IInboxTransport
    {
        private readonly IInMemoryTransport _inMemoryTransport;

        public InMemoryInboxWorkerTransport(IInMemoryTransport inMemoryTransport) 
        {
            _inMemoryTransport = inMemoryTransport;
        }

        public async IAsyncEnumerable<byte[]> GetMessages(IEnumerable<string> topics, CancellationToken cancellationToken)
        {
            var topicsWithWildcard = topics.Where(x => x.EndsWith('*'));

            var expectedTopicsStartWith = topicsWithWildcard.Select(x => x.Replace("*", string.Empty));

            await foreach (var message in _inMemoryTransport.Reader.ReadAllAsync(cancellationToken))
            {
                if (topics.Contains(message.Topic) || expectedTopicsStartWith.Where(message.Topic.StartsWith).Any())
                {
                    yield return message.Payload;
                }
                else
                {
                    continue;
                }
            }
        }
    }
}
