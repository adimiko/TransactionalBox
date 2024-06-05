using TransactionalBox.Internals.Outbox.OutboxDefinitions;

namespace TransactionalBox
{
    /// <summary>
    /// Define the outbox message.
    /// </summary>
    public abstract class OutboxDefinition<TOutboxMessage> : IOutboxDefinition
        where TOutboxMessage : OutboxMessage
    {
        protected internal string? Receiver { get; protected set; } = null;

        string? IOutboxDefinition.Receiver => Receiver;
    }
}
