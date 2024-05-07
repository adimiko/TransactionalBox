using TransactionalBox.Outbox.Internals.Jobs;
using TransactionalBox.Outbox.Internals.Launchers.Settings;

namespace TransactionalBox.Outbox.Settings.OutboxWorker
{
    public sealed class CleanUpProcessedOutboxMessagesSettings : ICleanUpProcessedOutboxMessagesJobSettings, ICleanUpProcessedOutboxMessagesLauncherSettings
    {
        public int BatchSize { get; set; } = 10000;

        public int NumberOfInstances { get; set; } = 1;

        public TimeSpan DelayWhenBatchIsEmpty { get; set; } = TimeSpan.FromSeconds(5);

        public TimeSpan DelayWhenBatchIsNotFull { get; set; } = TimeSpan.FromSeconds(1);

        internal CleanUpProcessedOutboxMessagesSettings() { }
    }
}
