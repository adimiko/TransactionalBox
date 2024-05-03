namespace TransactionalBox.Outbox.Internals.Transport.InMemory
{
    internal sealed class InMemoryTransportMessageSizeSettings : ITransportMessageSizeSettings
    {
        public int OptimalTransportMessageSize { get; } = 1073741824;
    }
}
