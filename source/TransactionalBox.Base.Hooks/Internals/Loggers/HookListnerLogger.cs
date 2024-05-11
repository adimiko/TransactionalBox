using Microsoft.Extensions.Logging;

namespace TransactionalBox.Base.Hooks.Internals.Loggers
{
    internal sealed partial class HookListnerLogger : IHookListnerLogger
    {
        private readonly ILogger<HookListnerLogger> _logger;

        public HookListnerLogger(ILogger<HookListnerLogger> logger) => _logger = logger;

        [LoggerMessage(0, LogLevel.Information, "Started '{hookName}' ('{hookId}')")]
        public partial void Started(string hookName, Guid hookId);

        [LoggerMessage(0, LogLevel.Information, "Ended hook '{hookId}'")]
        public partial void Ended(Guid hookId);
    }
}
