using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TransactionalBox.BackgroundServiceBase.Internals;
using TransactionalBox.Internals;
using TransactionalBox.OutboxWorker.Internals.Jobs;

namespace TransactionalBox.OutboxWorker.Internals
{
    internal sealed class OutboxWorkerLauncher : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly IParallelExecutor _parallelExecutor;

        private readonly IOutboxOrchestratorSettings _settings;

        private readonly ITransactionalBoxLogger _logger;

        public OutboxWorkerLauncher(
            IServiceProvider serviceProvider,
            IParallelExecutor parallelExecutor,
            IOutboxOrchestratorSettings settings,
            ITransactionalBoxLogger logger)
        {
            _serviceProvider = serviceProvider;
            _parallelExecutor = parallelExecutor;
            _settings = settings;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //TODO log settings

            try
            {
                var tasks = _parallelExecutor.Run<MessageProcessingJob>(_settings.NumberOfOutboxProcessor, stoppingToken);

                await Task.WhenAll(tasks);
            }
            catch(OperationCanceledException) { }
            catch (Exception ex) 
            {
                _logger.Error(ex, "Error");
            }
        }
    }
}
