using TransactionalBox.Inbox.Internals.Jobs;

namespace TransactionalBox.Inbox.Settings
{
    public sealed class AddMessagesToInboxStorageSettings : IAddMessagesToInboxStorageJobSettings
    {
        public TimeSpan DefaultTimeToLiveIdempotencyKey { get; set; } = TimeSpan.FromDays(7);

        internal AddMessagesToInboxStorageSettings() { }
    }
}
