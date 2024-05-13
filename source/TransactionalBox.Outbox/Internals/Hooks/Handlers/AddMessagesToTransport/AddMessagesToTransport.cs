using TransactionalBox.Base.EventHooks;
using TransactionalBox.Internals;
using TransactionalBox.Outbox.Internals.Hooks.Events;
using TransactionalBox.Outbox.Internals.Hooks.Handlers.AddMessagesToTransport.TransportMessageFactories;
using TransactionalBox.Outbox.Internals.Storage;
using TransactionalBox.Outbox.Internals.Transport;

namespace TransactionalBox.Outbox.Internals.Hooks.Handlers.AddMessagesToTransport
{
    internal sealed class AddMessagesToTransport : IEventHookHandler<AddedMessagesToOutbox>
    {
        private const int ErrorBatchSize = 1;

        private readonly IEventHookPublisher _eventHookPublisher;

        private readonly TransportMessageFactory _factory;

        private readonly IAddMessagesToTransportSettings _settings;

        private readonly ISystemClock _clock;

        private readonly IOutboxWorkerStorage _storage;

        private readonly IOutboxWorkerTransport _transport;

        public AddMessagesToTransport(
            IEventHookPublisher eventHookPublisher,
            TransportMessageFactory factory,
            IAddMessagesToTransportSettings settings,
            ISystemClock systemClock,
            IOutboxWorkerStorage storage,
            IOutboxWorkerTransport transport)
        {
            _eventHookPublisher = eventHookPublisher;
            _factory = factory;
            _settings = settings;
            _clock = systemClock;
            _storage = storage;
            _transport = transport;
        }

        public async Task HandleAsync(IHookExecutionContext context, CancellationToken cancellationToken)
        {
            var batchSize = _settings.BatchSize;

            if (context.IsError)
            {
                batchSize = ErrorBatchSize;
            }

            var firstIteration = true;
            var numberOfMessages = 0;

            do
            {
                if (!firstIteration)
                {
                    batchSize = _settings.BatchSize;
                }

                numberOfMessages = await _storage.MarkMessages(context.Id, context.Name, batchSize, _clock.TimeProvider, _settings.LockTimeout).ConfigureAwait(false);

                if (numberOfMessages == 0)
                {
                    return;
                }

                var messages = await _storage.GetMarkedMessages(context.Id).ConfigureAwait(false);

                var transportMessages = await _factory.Create(messages).ConfigureAwait(false);

                foreach (var transportMessage in transportMessages)
                {
                    await _transport.Add(transportMessage.Topic, transportMessage.Payload).ConfigureAwait(false);
                }

                await _storage.MarkAsProcessed(context.Id, _clock.UtcNow).ConfigureAwait(false);

                await _eventHookPublisher.PublishAsync<AddedMessagesToTransport>().ConfigureAwait(false);

                firstIteration = false;
            }
            while (!cancellationToken.IsCancellationRequested && numberOfMessages >= batchSize);
        }
    }
}
