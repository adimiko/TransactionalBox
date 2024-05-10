using Microsoft.Extensions.DependencyInjection;
using System.Runtime;
using TransactionalBox.Base.BackgroundService.Internals.Contexts.JobExecution;
using TransactionalBox.Base.BackgroundService.Internals.Contexts.JobExecution.ValueObjects;
using TransactionalBox.Base.BackgroundService.Internals.JobExecutors;
using TransactionalBox.Base.Hooks;
using TransactionalBox.Internals;
using TransactionalBox.Outbox.Internals.Jobs.AddMessagesToTransportJob;
using TransactionalBox.Outbox.Internals.Jobs.AddMessagesToTransportJob.TransportMessageFactories;
using TransactionalBox.Outbox.Internals.Storage;
using TransactionalBox.Outbox.Internals.Transport;

namespace TransactionalBox.Outbox.Internals.Hooks
{
    internal sealed class AddMessagesToTransportHook : Hook
    {
        private readonly IHookListener<AddMessagesToTransportHook> _hookListener;

        private readonly TransportMessageFactory _factory;

        private readonly IAddMessagesToTransportJobSettings _settings;

        private readonly ISystemClock _systemClock;

        private readonly IServiceScopeFactory _serviceScopeFactory;

        public AddMessagesToTransportHook(
            IHookListener<AddMessagesToTransportHook> hookListener,
            TransportMessageFactory factory,
            IAddMessagesToTransportJobSettings settings,
            ISystemClock systemClock,
            IServiceScopeFactory serviceScopeFactory)
        {
            _hookListener = hookListener;
            _factory = factory;
            _settings = settings;
            _systemClock = systemClock;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected internal override async Task StartAsync(CancellationToken cancellationToken)
        {
            //TODO execute on startup

            await foreach (var lastHook in _hookListener.ListenAsync(cancellationToken).ConfigureAwait(false))
            {
                //TODO logic
                await Process(cancellationToken);
            }

            //TODO execute on end
        }

        private async Task Process(CancellationToken cancellationToken)
        {
            using var scope = _serviceScopeFactory.CreateScope();

            var storage = scope.ServiceProvider.GetRequiredService<IOutboxWorkerStorage>();
            var transport = scope.ServiceProvider.GetRequiredService<IOutboxWorkerTransport>();

            //TODO 
            var jobId = new JobId(Guid.NewGuid().ToString());
            var jobName = new JobName(nameof(AddMessagesToTransportHook));

            var batchSize = _settings.BatchSize;

            var numberOfMessages = await storage.MarkMessages(jobId, jobName, batchSize, _systemClock.TimeProvider, _settings.LockTimeout);

            if (numberOfMessages == 0) // IsBatchEmpty
            {
                return;
            }

            var messages = await storage.GetMarkedMessages(jobId);

            var transportMessages = await _factory.Create(messages);

            foreach (var transportMessage in transportMessages)
            {
                var transportResult = await transport.Add(transportMessage.Topic, transportMessage.Payload);

                if (transportResult == TransportResult.Failure)
                {
                    //TODO log
                    return;
                }
                
                await storage.MarkAsProcessed(jobId, _systemClock.UtcNow);
            }
        }
    }
}
