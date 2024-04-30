using TransactionalBox.Outbox.Internals.Storage;

namespace TransactionalBox.Outbox.Internals
{
    internal interface IOutboxStorage
    {
        Task Add(OutboxMessage message);

        Task AddRange(IEnumerable<OutboxMessage> messages);
    }
}
