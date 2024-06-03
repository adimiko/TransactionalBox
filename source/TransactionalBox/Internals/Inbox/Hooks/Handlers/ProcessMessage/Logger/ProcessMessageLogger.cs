using Microsoft.Extensions.Logging;

namespace TransactionalBox.Internals.Inbox.Hooks.Handlers.ProcessMessage.Logger
{
    internal sealed partial class ProcessMessageLogger : IProcessMessageLogger
    {
        private readonly ILogger _logger;

        public ProcessMessageLogger(ILogger<ProcessMessage> logger) => _logger = logger;

        [LoggerMessage(0, LogLevel.Information, "{eventHookHandlerName} '{hookId}' processed message with id '{messageId}'", SkipEnabledCheck = true)]
        public partial void Processed(string eventHookHandlerName, Guid hookId, Guid messageId);
    }
}
