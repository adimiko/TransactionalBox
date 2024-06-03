using TransactionalBox.Inbox.Internals.Transport.ContractsToImplement;

namespace TransactionalBox.Inbox.Internals.Transport.InMemory
{
    internal sealed class InMemoryTransportTopicsCreator : ITransportTopicsCreator
    {
        public Task Create(IEnumerable<string> topics) => Task.CompletedTask;
    }
}
