using Microsoft.Extensions.Hosting;

namespace TransactionalBox.Inbox.Internals.BackgroundProcesses.Base
{
    internal abstract class BackgroundProcessBase : BackgroundService
    {
        protected sealed override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested) 
            {
                try
                {
                    await Process(stoppingToken).ConfigureAwait(false);
                }
                catch (Exception) 
                {
                    //TODO logger
                    //TODO delay
                    await Task.Delay(TimeSpan.FromMicroseconds(250), TimeProvider.System, stoppingToken).ConfigureAwait(false);
                }
            }
        }

        protected abstract Task Process(CancellationToken stoppingToken);
    }
}
