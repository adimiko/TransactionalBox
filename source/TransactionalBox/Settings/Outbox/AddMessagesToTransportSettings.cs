using TransactionalBox.Internals.Outbox.Hooks.Handlers.AddMessagesToTransport;

namespace TransactionalBox.Settings.Outbox
{
    public sealed class AddMessagesToTransportSettings : IAddMessagesToTransportSettings
    {
        public int MaxBatchSize { get; set; } = 5000;

        public TimeSpan LockTimeout { get; set; } = TimeSpan.FromSeconds(10);

        internal AddMessagesToTransportSettings() { }
    }
}
