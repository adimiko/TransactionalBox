using Microsoft.Extensions.Logging;

namespace TransactionalBox.Outbox.Internals.Hooks.Handlers.AddMessagesToTransport.Loggers
{
    internal sealed partial class AddMessagesToTransportLogger : IAddMessagesToTransportLogger
    {
        private readonly ILogger _logger;

        public AddMessagesToTransportLogger(ILogger<AddMessagesToTransport> logger) => _logger = logger;

        [LoggerMessage(0, LogLevel.Information, "{eventHookHandlerName} '{hookId}' (Iteration: {iteration} NumberOfMessages: {numberOfMessages} MaxBatchSize: {maxbatchSize})", SkipEnabledCheck = true)]
        public partial void Added(string eventHookHandlerName, Guid hookId, long iteration, int numberOfMessages, int maxBatchSize);
    }
}
