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

        private readonly IAddMessagesToTransportRepository _repository;

        private readonly IOutboxTransport _transport;

        public AddMessagesToTransport(
            IEventHookPublisher eventHookPublisher,
            TransportMessageFactory factory,
            IAddMessagesToTransportSettings settings,
            ISystemClock systemClock,
            IAddMessagesToTransportRepository repository,
            IOutboxTransport transport)
        {
            _eventHookPublisher = eventHookPublisher;
            _factory = factory;
            _settings = settings;
            _clock = systemClock;
            _repository = repository;
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

                numberOfMessages = await _repository.MarkMessages(context.Id, context.Name, batchSize, _clock.TimeProvider, _settings.LockTimeout).ConfigureAwait(false);

                if (numberOfMessages == 0)
                {
                    return;
                }

                var messages = await _repository.GetMarkedMessages(context.Id).ConfigureAwait(false);

                var transportMessages = await _factory.Create(messages).ConfigureAwait(false);

                foreach (var transportMessage in transportMessages)
                {
                    await _transport.Add(transportMessage.Topic, transportMessage.Payload).ConfigureAwait(false);
                }

                await _repository.MarkAsProcessed(context.Id, _clock.UtcNow).ConfigureAwait(false);

                //TODO logg numberOfMessages and iteration number

                await _eventHookPublisher.PublishAsync<AddedMessagesToTransport>().ConfigureAwait(false);

                firstIteration = false;
            }
            while (!cancellationToken.IsCancellationRequested && numberOfMessages >= batchSize);
        }
    }
}
