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
            LaunchJob<ProcessMessageFromInbox>(processMessageFromInboxSettings.NumberOfInstances);
            LaunchLongRunningJob<AddMessagesToInboxStorage>(addMessagesToInboxStorageLauncherSettings.NumberOfInstances);
            LaunchJob<CleanUpProcessedInboxMessages>(cleanUpProcessedInboxMessagesLauncherSettings.NumberOfInstances);
            LaunchJob<CleanUpExpiredIdempotencyKeys>(cleanUpExpiredIdempotencyKeysLauncherSettings.NumberOfInstances);
        }
    }
}
