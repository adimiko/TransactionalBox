using Microsoft.Extensions.Logging;

namespace TransactionalBox.Internals.Inbox.BackgroundProcesses.AddMessagesToInbox.Logger
{
    internal sealed partial class AddMessagesToInboxLogger : IAddMessagesToInboxLogger
    {
        private readonly ILogger _logger;

        public AddMessagesToInboxLogger(ILogger<AddMessagesToInbox> logger) => _logger = logger;


        [LoggerMessage(0, LogLevel.Error, "{name} (Attempt: {attempt} Delay: {msDelay}ms) unexpected exception", SkipEnabledCheck = true)]
        public partial void UnexpectedException(string name, long attempt, long msDelay, Exception exception);
    }
}
