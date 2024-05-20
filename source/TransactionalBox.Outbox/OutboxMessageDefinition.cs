using TransactionalBox.Outbox.Internals.OutboxMessageDefinitions;

namespace TransactionalBox.Outbox
{
    public abstract class OutboxMessageDefinition<TOutboxMessage> : IOutboxMessageDefinition
        where TOutboxMessage : OutboxMessage
    {
        protected internal string? Receiver { get; protected set; } = null;

        string? IOutboxMessageDefinition.Receiver => Receiver;
    }
}
