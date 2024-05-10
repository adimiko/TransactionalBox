using TransactionalBox.Outbox.Internals.Hooks.AddMessagesToTransport;
using TransactionalBox.Outbox.Internals.Launchers.Settings;

namespace TransactionalBox.Outbox.Settings
{
    public sealed class AddMessagesToTransportSettings : IAddMessagesToTransportHookSettings
    {
        public int BatchSize { get; set; } = 5000;

        public TimeSpan LockTimeout { get; set; } = TimeSpan.FromSeconds(10);

        internal AddMessagesToTransportSettings() { }
    }
}
