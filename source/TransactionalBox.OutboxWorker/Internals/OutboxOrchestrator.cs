using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TransactionalBox.BackgroundServiceBase.Internals;

namespace TransactionalBox.OutboxWorker.Internals
{
    internal sealed class OutboxOrchestrator : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly IParallelExecutor _parallelExecutor;

        private readonly IOutboxOrchestratorSettings _settings;

        public OutboxOrchestrator(
            IServiceProvider serviceProvider,
            IParallelExecutor parallelExecutor,
            IOutboxOrchestratorSettings settings) 
        {
            _serviceProvider = serviceProvider;
            _parallelExecutor = parallelExecutor;
            _settings = settings;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //TODO log settings

            try
            {
                var tasks = _parallelExecutor.Run<OutboxProcessor>(_settings.NumberOfOutboxProcessor, stoppingToken);

                await Task.WhenAll(tasks);
            }
            catch(OperationCanceledException) { }
            catch (Exception ex) 
            {
                //TODO log
            }
        }
    }
}
