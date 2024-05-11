using TransactionalBox.Base.BackgroundService.Internals.Contexts.JobExecution.ValueObjects;
using TransactionalBox.Base.EventHooks;
using TransactionalBox.Internals;
using TransactionalBox.Outbox.Internals.Hooks.AddMessagesToTransport.TransportMessageFactories;
using TransactionalBox.Outbox.Internals.Hooks.CleanUpProcessedOutboxMessages;
using TransactionalBox.Outbox.Internals.Hooks.EventHooks;
using TransactionalBox.Outbox.Internals.Loggers;
using TransactionalBox.Outbox.Internals.Storage;
using TransactionalBox.Outbox.Internals.Transport;

namespace TransactionalBox.Outbox.Internals.Hooks.AddMessagesToTransport
{
    internal sealed class AddMessagesToTransport : IEventHookHandler<AddedMessagesToOutboxEventHook>
    {
        private readonly IEventHookPublisher _eventHookPublisher;

        private readonly TransportMessageFactory _factory;

        private readonly IAddMessagesToTransportHookSettings _settings;

        private readonly ISystemClock _clock;

        private readonly IOutboxWorkerStorage _storage;

        private readonly IOutboxWorkerTransport _transport;

        private readonly IOutboxWorkerLogger<AddMessagesToTransport> _logger;

        public AddMessagesToTransport(
            IEventHookPublisher eventHookPublisher,
            TransportMessageFactory factory,
            IAddMessagesToTransportHookSettings settings,
            ISystemClock systemClock,
            IOutboxWorkerStorage storage,
            IOutboxWorkerTransport transport,
            IOutboxWorkerLogger<AddMessagesToTransport> logger)
        {
            _eventHookPublisher = eventHookPublisher;
            _factory = factory;
            _settings = settings;
            _clock = systemClock;
            _storage = storage;
            _transport = transport;
            _logger = logger;
        }

        public async Task HandleAsync(IHookExecutionContext context, CancellationToken cancellationToken)
        {
            var batchSize = _settings.BatchSize;

            var numberOfMessages = await _storage.MarkMessages(context.Id, context.Name, batchSize, _clock.TimeProvider, _settings.LockTimeout).ConfigureAwait(false);

            //TODO check when numberofMessages is equal batchSize repeat
            if (numberOfMessages == 0) // IsBatchEmpty
            {
                return;
            }

            var messages = await _storage.GetMarkedMessages(context.Id);

            var transportMessages = await _factory.Create(messages).ConfigureAwait(false);

            foreach (var transportMessage in transportMessages)
            {
                var transportResult = await _transport.Add(transportMessage.Topic, transportMessage.Payload).ConfigureAwait(false);

                if (transportResult == TransportResult.Failure)
                {
                    //TODO retry 
                    _logger.FailedToAddMessagesToTransport();
                    return;
                }

                await _storage.MarkAsProcessed(context.Id, _clock.UtcNow).ConfigureAwait(false);
            }

            await _eventHookPublisher.PublishAsync<AddedMessagesToTransportEventHook>().ConfigureAwait(false);
        }
    }
}
