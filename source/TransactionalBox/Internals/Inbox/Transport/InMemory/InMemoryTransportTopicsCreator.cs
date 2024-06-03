using TransactionalBox.Internals.Inbox.Transport.ContractsToImplement;

namespace TransactionalBox.Internals.Inbox.Transport.InMemory
{
    internal sealed class InMemoryTransportTopicsCreator : ITransportTopicsCreator
    {
        public Task Create(IEnumerable<string> topics) => Task.CompletedTask;
    }
}
