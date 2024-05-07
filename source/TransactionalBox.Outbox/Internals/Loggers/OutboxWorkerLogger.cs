using Microsoft.Extensions.Logging;

namespace TransactionalBox.Outbox.Internals.Loggers
{
    internal sealed partial class OutboxWorkerLogger<T> : IOutboxWorkerLogger<T>
    {
        private readonly ILogger<T> _logger;

        public OutboxWorkerLogger(ILogger<T> logger)
        {
            _logger = logger;
        }

        [LoggerMessage(0, LogLevel.Error, "Failed to add messages to transport", SkipEnabledCheck = true)]
        public partial void FailedToAddMessagesToTransport();
    }
}
