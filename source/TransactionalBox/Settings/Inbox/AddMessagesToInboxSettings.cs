using TransactionalBox.Internals.Inbox.BackgroundProcesses.AddMessagesToInbox;

namespace TransactionalBox.Settings.Inbox
{
    public sealed class AddMessagesToInboxSettings : IAddMessagesToInboxSettings
    {
        public TimeSpan DefaultTimeToLiveIdempotencyKey { get; set; } = TimeSpan.FromDays(7);

        internal AddMessagesToInboxSettings() { }
    }
}
