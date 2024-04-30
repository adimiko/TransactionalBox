using TransactionalBox.Outbox.Internals.Contracts;

namespace TransactionalBox.Base.Transport.InMemory.Internals
{
    internal sealed class InMemoryTransportMessageSizeSettings : ITransportMessageSizeSettings
    {
        public int OptimalTransportMessageSize { get; } = 1073741824;
    }
}
