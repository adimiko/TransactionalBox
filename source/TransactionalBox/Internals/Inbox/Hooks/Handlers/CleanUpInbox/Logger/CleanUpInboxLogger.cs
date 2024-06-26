﻿using Microsoft.Extensions.Logging;

namespace TransactionalBox.Internals.Inbox.Hooks.Handlers.CleanUpInbox.Logger
{
    internal sealed partial class CleanUpInboxLogger : ICleanUpInboxLogger
    {
        private readonly ILogger _logger;

        public CleanUpInboxLogger(ILogger<CleanUpInbox> logger) => _logger = logger;


        [LoggerMessage(0, LogLevel.Information, "{eventHookHandlerName} '{hookId}' (Iteration: {iteration} NumberOfMessages: {numberOfMessages})", SkipEnabledCheck = true)]
        public partial void CleanedUp(string eventHookHandlerName, Guid hookId, long iteration, int numberOfMessages);
    }
}
