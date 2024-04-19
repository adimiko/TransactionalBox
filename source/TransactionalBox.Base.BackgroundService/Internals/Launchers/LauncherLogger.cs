using Microsoft.Extensions.Logging;
using TransactionalBox.Base.BackgroundService.Internals.Launchers;

namespace TransactionalBox.Base.BackgroundService.Internals.Loggers
{
    internal sealed partial class LauncherLogger<T> : ILauncherLogger<T>
        where T : class
    {
        private readonly ILogger<T> _logger;

        public LauncherLogger(ILogger<T> logger) => _logger = logger;

        [LoggerMessage(0, LogLevel.Error, "Unexpected exception", SkipEnabledCheck = true)]
        public partial void UnexpectedError(Exception exception);
    }
}
