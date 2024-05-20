using TransactionalBox.Inbox.Internals.Transport.Topics;

namespace TransactionalBox.Inbox.Internals.Transport.InMemory
{
    internal sealed class InMemoryTransportTopicsCreator : ITransportTopicsCreator
    {
        public Task Create(IEnumerable<string> topics) => Task.CompletedTask;
    }
}
