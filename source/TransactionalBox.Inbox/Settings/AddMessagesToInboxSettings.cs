using TransactionalBox.Inbox.Internals.BackgroundProcesses.AddMessagesToInbox;

namespace TransactionalBox.Inbox.Settings
{
    public sealed class AddMessagesToInboxSettings : IAddMessagesToInboxSettings
    {
        public TimeSpan DefaultTimeToLiveIdempotencyKey { get; set; } = TimeSpan.FromDays(7);

        internal AddMessagesToInboxSettings() { }
    }
}
