using Microsoft.Extensions.Logging;

namespace TransactionalBox.Inbox.Internals.BackgroundProcesses.CleanUpIdempotencyKeys.Logger
{
    internal sealed partial class CleanUpIdempotencyKeysLogger : ICleanUpIdempotencyKeysLogger
    {
        private readonly ILogger _logger;

        public CleanUpIdempotencyKeysLogger(ILogger<CleanUpIdempotencyKeys> logger) => _logger = logger;

        [LoggerMessage(0, LogLevel.Information, "{name} '{id}' (Iteration: {iteration} NumberOfMessages: {numberOfMessages})", SkipEnabledCheck = true)]
        public partial void CleanedUp(string name, Guid id, long iteration, int numberOfMessages);

        [LoggerMessage(0, LogLevel.Error, "{name} (Attempt: {attempt} Delay: {msDelay}ms) unexpected exception", SkipEnabledCheck = true)]
        public partial void UnexpectedException(string name, long attempt, long msDelay, Exception exception);

    }
}
