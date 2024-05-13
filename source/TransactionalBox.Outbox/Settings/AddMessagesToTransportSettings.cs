using TransactionalBox.Outbox.Internals.Hooks.Handlers.AddMessagesToTransport;

namespace TransactionalBox.Outbox.Settings
{
    public sealed class AddMessagesToTransportSettings : IAddMessagesToTransportSettings
    {
        public int MaxBatchSize { get; set; } = 5000;

        public TimeSpan LockTimeout { get; set; } = TimeSpan.FromSeconds(10);

        internal AddMessagesToTransportSettings() { }
    }
}
