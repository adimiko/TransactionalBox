using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Base.BackgroundService.Internals.Contexts.JobExecution.ValueObjects;
using TransactionalBox.Base.Hooks;
using TransactionalBox.Internals;
using TransactionalBox.Outbox.Internals.Hooks.AddMessagesToTransport.TransportMessageFactories;
using TransactionalBox.Outbox.Internals.Hooks.CleanUpProcessedOutboxMessages;
using TransactionalBox.Outbox.Internals.Loggers;
using TransactionalBox.Outbox.Internals.Storage;
using TransactionalBox.Outbox.Internals.Transport;

namespace TransactionalBox.Outbox.Internals.Hooks.AddMessagesToTransport
{
    internal sealed class AddMessagesToTransportHook : Hook
    {
        private readonly IHookListener<AddMessagesToTransportHook> _hookListener;

        private readonly IHookCaller<CleanUpProcessedOutboxMessagesHook> _hookCaller;

        private readonly TransportMessageFactory _factory;

        private readonly IAddMessagesToTransportHookSettings _settings;

        private readonly ISystemClock _systemClock;

        private readonly IServiceScopeFactory _serviceScopeFactory;

        public AddMessagesToTransportHook(
            IHookListener<AddMessagesToTransportHook> hookListener,
            IHookCaller<CleanUpProcessedOutboxMessagesHook> hookCaller,
            TransportMessageFactory factory,
            IAddMessagesToTransportHookSettings settings,
            ISystemClock systemClock,
            IServiceScopeFactory serviceScopeFactory)
        {
            _hookListener = hookListener;
            _hookCaller = hookCaller;
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
                await Process(cancellationToken).ConfigureAwait(false);

                await _hookCaller.CallAsync().ConfigureAwait(false);
            }

            //TODO execute on end
        }

        private async Task Process(CancellationToken cancellationToken)
        {
            using var scope = _serviceScopeFactory.CreateScope();

            var storage = scope.ServiceProvider.GetRequiredService<IOutboxWorkerStorage>();
            var transport = scope.ServiceProvider.GetRequiredService<IOutboxWorkerTransport>();
            var logger = scope.ServiceProvider.GetRequiredService<IOutboxWorkerLogger<AddMessagesToTransportHook>>();

            //TODO 
            var jobId = new JobId(Guid.NewGuid().ToString());
            var jobName = new JobName(nameof(AddMessagesToTransportHook));

            var batchSize = _settings.BatchSize;

            var numberOfMessages = await storage.MarkMessages(jobId, jobName, batchSize, _systemClock.TimeProvider, _settings.LockTimeout);

            //TODO check when numberofMessages is equal batchSize repeat
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
                    logger.FailedToAddMessagesToTransport();
                    return;
                }

                await storage.MarkAsProcessed(jobId, _systemClock.UtcNow);
            }
        }
    }
}
