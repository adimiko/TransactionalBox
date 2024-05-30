using TransactionalBox.Outbox.Internals.Oubox;
using TransactionalBox.Outbox.Internals.OutboxMessageDefinitions;

namespace TransactionalBox
{
    /// <summary>
    /// Define the outbox message.
    /// </summary>
    public abstract class OutboxMessageDefinition<TOutboxMessage> : IOutboxMessageDefinition
        where TOutboxMessage : OutboxMessage
    {
        protected internal string? Receiver { get; protected set; } = null;

        string? IOutboxMessageDefinition.Receiver => Receiver;
    }
}
