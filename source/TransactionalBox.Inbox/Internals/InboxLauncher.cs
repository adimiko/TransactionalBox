using TransactionalBox.BackgroundServiceBase.Internals;
using TransactionalBox.Inbox.Internals.Jobs;

namespace TransactionalBox.Inbox.Internals
{
    internal sealed class InboxLauncher : Launcher
    {
        public InboxLauncher(IServiceProvider serviceProvider) 
            : base(serviceProvider)
        {
            Launch<ProcessMessageFromInboxStorage>(4);//TODO;
        }
    }
}
