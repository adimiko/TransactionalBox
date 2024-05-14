using Microsoft.Extensions.Logging;

namespace TransactionalBox.Inbox.Internals.BackgroundProcesses.AddMessagesToInbox.Logger
{
    internal sealed partial class AddMessagesToInboxLogger : IAddMessagesToInboxLogger
    {
        private readonly ILogger _logger;

        public AddMessagesToInboxLogger(ILogger<AddMessagesToInbox> logger) => _logger = logger;


        [LoggerMessage(0, LogLevel.Error, "Unexpected exception", SkipEnabledCheck = true)]
        public partial void UnexpectedException(Exception exception);
    }
}
