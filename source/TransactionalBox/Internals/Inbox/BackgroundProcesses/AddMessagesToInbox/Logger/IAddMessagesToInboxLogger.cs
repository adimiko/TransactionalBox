using TransactionalBox.Internals.Inbox.BackgroundProcesses.Base.Logger;

namespace TransactionalBox.Internals.Inbox.BackgroundProcesses.AddMessagesToInbox.Logger
{
    internal interface IAddMessagesToInboxLogger : IBackgroundProcessBaseLogger
    {
        void DetectedDuplicatedMessages(string ids);
    }
}
