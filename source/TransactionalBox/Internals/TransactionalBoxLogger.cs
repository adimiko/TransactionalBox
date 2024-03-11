using Microsoft.Extensions.Logging;
using System;

namespace TransactionalBox.Internals
{
    internal sealed class TransactionalBoxLogger : ITransactionalBoxLogger
    {
        private readonly ILogger<TransactionalBoxLogger> _logger;

        public TransactionalBoxLogger(ILogger<TransactionalBoxLogger> logger)
        {
            _logger = logger;
        }

        public void Critical(string? message, params object?[] args)
        {
            if (_logger.IsEnabled(LogLevel.Critical))
            {
                _logger.LogCritical(message, args);
            }
        }

        public void Critical(Exception exception, string? message, params object?[] args)
        {
            if (_logger.IsEnabled(LogLevel.Critical))
            {
                _logger.LogCritical(exception, message, args);
            }
        }

        public void Debug(string? message, params object?[] args)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug(message, args);
            }
        }

        public void Debug(Exception exception, string? message, params object?[] args)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug(exception, message, args);
            }
        }

        public void Error(string? message, params object?[] args)
        {
            if (_logger.IsEnabled(LogLevel.Error)) 
            {
                _logger.LogError(message, args);
            }
        }

        public void Error(Exception exception, string? message, params object?[] args)
        {
            if (_logger.IsEnabled(LogLevel.Error))
            {
                _logger.LogError(exception, message, args);
            }
        }

        public void Information(string? message, params object?[] args)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation(message, args);
            }
        }

        public void Information(Exception exception, string? message, params object?[] args)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation(exception, message, args);
            }
        }

        public void Trace(string? message, params object?[] args)
        {
            if (_logger.IsEnabled(LogLevel.Trace))
            {
                _logger.LogTrace(message, args);
            }
        }

        public void Trace(Exception exception, string? message, params object?[] args)
        {
            if (_logger.IsEnabled(LogLevel.Trace))
            {
                _logger.LogTrace(exception, message, args);
            }
        }

        public void Warning(string? message, params object?[] args)
        {
            if (_logger.IsEnabled(LogLevel.Warning))
            {
                _logger.LogWarning(message, args);
            }
        }

        public void Warning(Exception exception, string? message, params object?[] args)
        {
            if (_logger.IsEnabled(LogLevel.Warning))
            {
                _logger.LogWarning(exception, message, args);
            }
        }
    }
}
