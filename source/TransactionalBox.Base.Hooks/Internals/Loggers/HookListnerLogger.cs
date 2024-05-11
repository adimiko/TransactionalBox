using Microsoft.Extensions.Logging;

namespace TransactionalBox.Base.Hooks.Internals.Loggers
{
    internal sealed partial class HookListnerLogger<THook> : IHookListnerLogger<THook>
        where THook : Hook, new()
    {
        private readonly ILogger<THook> _logger;

        public HookListnerLogger(ILogger<THook> logger) => _logger = logger;

        [LoggerMessage(0, LogLevel.Information, "Started '{hookName}' ('{hookId}')", SkipEnabledCheck = true)]
        public partial void Started(string hookName, Guid hookId);

        [LoggerMessage(0, LogLevel.Information, "Ended hook '{hookId}'", SkipEnabledCheck = true)]
        public partial void Ended(Guid hookId);

        [LoggerMessage(0, LogLevel.Error, "Unexpected exception", SkipEnabledCheck = true)]
        public partial void UnexpectedError(Exception exception);
    }
}
