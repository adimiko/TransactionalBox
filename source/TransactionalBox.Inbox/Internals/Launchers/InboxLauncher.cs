﻿using TransactionalBox.Base.BackgroundService.Internals.Launchers;
using TransactionalBox.Inbox.Internals.Jobs;

namespace TransactionalBox.Inbox.Internals.Launchers
{
    internal sealed class InboxLauncher : Launcher
    {
        public InboxLauncher(
            IServiceProvider serviceProvider,
            IInboxLauncherSettings settings)
            : base(serviceProvider)
        {
            Launch<ProcessMessageFromInbox>(settings.NumberOfInstances);
        }
    }
}
