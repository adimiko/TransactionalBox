using TransactionalBox.OutboxWorker.Internals.Jobs;
using TransactionalBox.OutboxWorker.Internals.Launchers;

namespace TransactionalBox.OutboxWorker.Settings
{
    public sealed class CleanUpProcessedMessagesSettings : ICleanUpProcessedMessagesJobSettings, ICleanUpProcessedMessagesLauncherSettings
    {
        public int BatchSize { get; set; } = 10000;

        public int NumberOfInstances { get; set; } = 1;

        public TimeSpan DelayWhenBatchIsEmpty { get; set; } = TimeSpan.FromSeconds(5);

        public TimeSpan DelayWhenBatchIsNotFull { get; set; } = TimeSpan.FromSeconds(1);

        internal CleanUpProcessedMessagesSettings() { }
    }
}
