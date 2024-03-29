using TransactionalBox.BackgroundServiceBase.Internals;
using TransactionalBox.Inbox.Internals.Jobs;
using TransactionalBox.Inbox.Internals.Settings;

namespace TransactionalBox.Inbox.Internals
{
    internal sealed class InboxLauncher : Launcher
    {
        public InboxLauncher(
            IServiceProvider serviceProvider,
            IInboxLauncherSettings settings) 
            : base(serviceProvider)
        {
            Launch<ProcessMessageFromInboxStorage>(settings.NumberOfProcessMessageFromInboxStorageExecutors);
        }
    }
}
