using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Internals;

namespace TransactionalBox.BackgroundServiceBase.Internals
{
    //TODO internal
    internal sealed class JobExecutor
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly ITransactionalBoxLogger _logger;

        public JobExecutor(
            IServiceProvider serviceProvider,
            ITransactionalBoxLogger logger) 
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        internal async Task Execute(Type jobType, string jobId, CancellationToken stoppingToken)
        {
            //TODO prepare
            //TODO log settings & enviroment (ProcessorCount etc.)
            //TODO error

            //_logger.Information("Settings: {0}", _settings);
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        Job job = scope.ServiceProvider.GetRequiredService(jobType) as Job;

                        await job.Execute(jobId, stoppingToken);
                    }
                }
            }
            catch (Exception ex) 
            {
                _logger.Error(ex, "Error");
            }
        }

    }
}
