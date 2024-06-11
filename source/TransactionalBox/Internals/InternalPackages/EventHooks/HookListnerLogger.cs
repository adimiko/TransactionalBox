using Microsoft.Extensions.Logging;
using TransactionalBox.Internals.InternalPackages.EventHooks;

namespace TransactionalBox.Internals.EventHooks.Internals.Loggers
{
    internal sealed partial class HookListnerLogger<THook> : IHookListnerLogger<THook>
        where THook : EventHook, new()
    {
        private readonly ILogger<THook> _logger;

        public HookListnerLogger(ILogger<THook> logger) => _logger = logger;

        [LoggerMessage(0, LogLevel.Information, "{eventHookHandlerName} '{hookId}' started", SkipEnabledCheck = true)]
        public partial void Started(string eventHookHandlerName, Guid hookId);

        [LoggerMessage(0, LogLevel.Information, "{eventHookHandlerName} '{hookId}' ended", SkipEnabledCheck = true)]
        public partial void Ended(string eventHookHandlerName, Guid hookId);

        [LoggerMessage(0, LogLevel.Error, "{eventHookHandlerName} '{hookId}' (Attempt: {attempt} Delay: {msDelay}ms) unexpected exception", SkipEnabledCheck = true)]
        public partial void UnexpectedException(string eventHookHandlerName, Guid hookId, long attempt, long msDelay, Exception exception);

        [LoggerMessage(0, LogLevel.Error, "Unexpected exception", SkipEnabledCheck = true)]
        public partial void UnexpectedException(Exception exception);
    }
}
