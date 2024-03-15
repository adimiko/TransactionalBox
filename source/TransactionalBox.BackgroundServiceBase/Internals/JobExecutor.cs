using Microsoft.Extensions.DependencyInjection;

namespace TransactionalBox.BackgroundServiceBase.Internals
{
    //TODO internal
    internal sealed class JobExecutor
    {
        private readonly IServiceProvider _serviceProvider;

        public JobExecutor(IServiceProvider serviceProvider) 
        {
            _serviceProvider = serviceProvider;
        }

        internal async Task Execute<T>(string jobId, CancellationToken stoppingToken)
            where T : Job
        {
            //TODO prepare
            //TODO log settings & enviroment (ProcessorCount etc.)
            //TODO error

            //_logger.Information("Settings: {0}", _settings);

            while (!stoppingToken.IsCancellationRequested) 
            {
                using (var scope = _serviceProvider.CreateScope()) 
                {
                    await scope.ServiceProvider.GetRequiredService<T>().Execute(jobId, stoppingToken);
                }
            }

            //TODO end
        }

    }
}
