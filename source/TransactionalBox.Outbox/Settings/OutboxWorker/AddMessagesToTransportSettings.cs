using TransactionalBox.Outbox.Internals.Jobs.AddMessagesToTransportJob;
using TransactionalBox.Outbox.Internals.Launchers.Settings;

namespace TransactionalBox.Outbox.Settings.OutboxWorker
{
    public sealed class AddMessagesToTransportSettings : IAddMessagesToTransportJobSettings, IAddMessagesToTransportLauncherSettings
    {
        public int BatchSize { get; set; } = 5000;

        public int NumberOfInstances { get; set; } = 2;

        public TimeSpan DelayWhenBatchIsEmpty { get; set; } = TimeSpan.FromMilliseconds(250);

        public TimeSpan DelayWhenBatchIsNotFull { get; set; } = TimeSpan.FromMilliseconds(100);

        public TimeSpan LockTimeout { get; set; } = TimeSpan.FromSeconds(10);

        internal AddMessagesToTransportSettings() { }
    }
}
