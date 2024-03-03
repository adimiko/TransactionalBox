using Microsoft.Extensions.Hosting;

namespace TransactionalBox.OutboxWorker.Internals
{
    internal sealed class OutboxProcessor : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //TODO prepare
            //TODO log settings & enviroment (ProcessorCount etc.)

            while (!stoppingToken.IsCancellationRequested)
            {
                //TODO logic
            }

            //TODO clean up

            return Task.CompletedTask;
        }
    }
}
