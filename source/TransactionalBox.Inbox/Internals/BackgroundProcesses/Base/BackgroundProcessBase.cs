using Microsoft.Extensions.Hosting;
using TransactionalBox.Inbox.Internals.BackgroundProcesses.Base.Logger;

namespace TransactionalBox.Inbox.Internals.BackgroundProcesses.Base
{
    internal abstract class BackgroundProcessBase : BackgroundService
    {
        private readonly IBackgroundProcessBaseLogger _logger;

        protected BackgroundProcessBase(IBackgroundProcessBaseLogger logger) 
        {
            _logger = logger;
        }

        protected sealed override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested) 
            {
                try
                {
                    await Process(stoppingToken).ConfigureAwait(false);
                }
                catch (Exception exception) 
                {
                    _logger.UnexpectedException(exception);

                    //TODO delay
                    await Task.Delay(TimeSpan.FromSeconds(1), TimeProvider.System, stoppingToken).ConfigureAwait(false);
                }
            }
        }

        protected abstract Task Process(CancellationToken stoppingToken);
    }
}
