using Microsoft.Extensions.Logging;

namespace TransactionalBox.Base.EventHooks.Internals.Loggers
{
    internal sealed partial class HookListnerLogger<THook> : IHookListnerLogger<THook>
        where THook : EventHook, new()
    {
        private readonly ILogger<THook> _logger;

        public HookListnerLogger(ILogger<THook> logger) => _logger = logger;

        [LoggerMessage(0, LogLevel.Information, "Started '{eventHookHandlerName}' ('{hookId}')", SkipEnabledCheck = true)]
        public partial void Started(string eventHookHandlerName, Guid hookId);

        [LoggerMessage(0, LogLevel.Information, "Ended '{hookId}'", SkipEnabledCheck = true)]
        public partial void Ended(Guid hookId);

        [LoggerMessage(0, LogLevel.Error, "Unexpected exception", SkipEnabledCheck = true)]
        public partial void UnexpectedError(Exception exception);
    }
}
