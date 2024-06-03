using TransactionalBox.Internals.Outbox.Transport.ContractsToImplement;

namespace TransactionalBox.Internals.Outbox.Transport.InMemory
{
    internal sealed class InMemoryTransportMessageSizeSettings : ITransportMessageSizeSettings
    {
        public int OptimalTransportMessageSize { get; } = 1073741824;
    }
}
