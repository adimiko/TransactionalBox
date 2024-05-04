using TransactionalBox.Base.BackgroundService.Internals.Launchers;
using TransactionalBox.Inbox.Internals.Jobs;

namespace TransactionalBox.Inbox.Internals.Launchers
{
    internal sealed class InboxLauncher : Launcher
    {
        public InboxLauncher(
            IServiceProvider serviceProvider,
            IProcessingMessagesFromInboxLauncherSettings processMessageFromInboxSettings,
            IAddMessagesToInboxStorageLauncherSettings addMessagesToInboxStorageLauncherSettings,
            ICleanUpProcessedInboxMessagesLauncherSettings cleanUpProcessedInboxMessagesLauncherSettings,
            ICleanUpExpiredIdempotencyKeysLauncherSettings cleanUpExpiredIdempotencyKeysLauncherSettings)
            : base(serviceProvider)
        {
            Launch<ProcessMessageFromInbox>(processMessageFromInboxSettings.NumberOfInstances);
            Launch<AddMessagesToInboxStorage>(addMessagesToInboxStorageLauncherSettings.NumberOfInstances);
            Launch<CleanUpProcessedInboxMessages>(cleanUpProcessedInboxMessagesLauncherSettings.NumberOfInstances);
            Launch<CleanUpExpiredIdempotencyKeys>(cleanUpExpiredIdempotencyKeysLauncherSettings.NumberOfInstances);
        }
    }
}
