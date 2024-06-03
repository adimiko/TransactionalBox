using Microsoft.Extensions.Logging;

namespace TransactionalBox.Internals.Outbox.Hooks.Handlers.CleanUpOutbox.Logger
{
    internal sealed partial class CleanUpOutboxLogger : ICleanUpOutboxLogger
    {
        private readonly ILogger _logger;

        public CleanUpOutboxLogger(ILogger<CleanUpOutbox> logger) => _logger = logger;

        [LoggerMessage(0, LogLevel.Information, "{eventHookHandlerName} '{hookId}' (Iteration: {iteration} NumberOfMessages: {numberOfMessages})", SkipEnabledCheck = true)]
        public partial void CleanedUp(string eventHookHandlerName, Guid hookId, long iteration, int numberOfMessages);
    }
}
