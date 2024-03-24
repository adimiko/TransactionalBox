using Microsoft.Extensions.Logging;

namespace TransactionalBox.BackgroundServiceBase.Internals.Loggers
{
    internal sealed class LauncherLogger<T> : ILauncherLogger<T>
        where T : class
    {
        private readonly ILogger<T> _logger;

        public LauncherLogger(ILogger<T> logger)
        {
            _logger = logger;
        }

        private static Action<ILogger, Exception?> _unexpectedException = LoggerMessage.Define(LogLevel.Error, 0, "Unexpected exception");

        public void UnexpectedError(Exception exception) => _unexpectedException(_logger, exception);
    }
}
