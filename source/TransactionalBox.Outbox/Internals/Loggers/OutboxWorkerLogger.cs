using Microsoft.Extensions.Logging;

namespace TransactionalBox.Outbox.Internals.Loggers
{
    internal sealed class OutboxWorkerLogger<T> : IOutboxWorkerLogger<T>
    {
        private readonly ILogger<T> _logger;

        public OutboxWorkerLogger(ILogger<T> logger)
        {
            _logger = logger;
        }

        private static Action<ILogger, Exception?> _failureAddToTransportAction = LoggerMessage.Define(LogLevel.Error, 0, "Failed to add messages to transport");

        public void FailedToAddMessagesToTransport() => _failureAddToTransportAction(_logger, null);
    }
}
