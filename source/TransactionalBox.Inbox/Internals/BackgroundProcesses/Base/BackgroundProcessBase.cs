using Microsoft.Extensions.Hosting;
using TransactionalBox.Inbox.Internals.BackgroundProcesses.Base.Logger;


namespace TransactionalBox.Inbox.Internals.BackgroundProcesses.Base
{
    internal abstract class BackgroundProcessBase
    {
        private readonly IBackgroundProcessBaseLogger _logger;

        protected BackgroundProcessBase(IBackgroundProcessBaseLogger logger) 
        {
            _logger = logger;
        }

        internal async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var name = GetType().Name;

            long attempt = 0;

            while (!stoppingToken.IsCancellationRequested) 
            {
                try
                {
                    await Process(stoppingToken).ConfigureAwait(false);

                    attempt = 0;
                }
                catch (Exception exception)
                {
                    attempt++;

                    long msDelay = attempt * 100;
                    const long maxMsDelay = 3000;

                    if (msDelay > maxMsDelay)
                    {
                        msDelay = maxMsDelay;
                    }

                    _logger.UnexpectedException(name, attempt, msDelay, exception);

                    await Task.Delay(TimeSpan.FromMilliseconds(msDelay), stoppingToken).ConfigureAwait(false);
                }
            }
        }

        protected abstract Task Process(CancellationToken stoppingToken);
    }
}
