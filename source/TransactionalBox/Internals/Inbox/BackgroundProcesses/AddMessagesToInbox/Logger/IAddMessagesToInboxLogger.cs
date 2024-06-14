using TransactionalBox.Internals.Inbox.BackgroundProcesses.Base.Logger;

namespace TransactionalBox.Internals.Inbox.BackgroundProcesses.AddMessagesToInbox.Logger
{
    internal interface IAddMessagesToInboxLogger : IBackgroundProcessBaseLogger
    {
        void AddedMessagesToInbox(int numberOfMessages);

        void DetectedDuplicatedMessages(string ids);
    }
}
