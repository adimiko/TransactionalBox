﻿using TransactionalBox.Internals.Outbox.Hooks.Handlers.AddMessagesToTransport.TransportMessageFactories;
using TransactionalBox.Internals.Outbox.Storage.ContractsToImplement;
using TransactionalBox.Internals.Outbox.Transport.ContractsToImplement;
using TransactionalBox.Internals.Outbox.Hooks.Events;
using TransactionalBox.Internals.Outbox.Hooks.Handlers.AddMessagesToTransport.Logger;
using TransactionalBox.Internals.InternalPackages.EventHooks;

namespace TransactionalBox.Internals.Outbox.Hooks.Handlers.AddMessagesToTransport
{
    internal sealed class AddMessagesToTransport : IEventHookHandler<AddedMessagesToOutbox>
    {
        private const int ErrorBatchSize = 1;

        private readonly IEventHookPublisher _eventHookPublisher;

        private readonly TransportEnvelopeFactory _factory;

        private readonly IAddMessagesToTransportSettings _settings;

        private readonly ISystemClock _clock;

        private readonly IAddMessagesToTransportRepository _repository;

        private readonly IOutboxTransport _transport;

        private readonly IAddMessagesToTransportLogger _logger;

        public AddMessagesToTransport(
            IEventHookPublisher eventHookPublisher,
            TransportEnvelopeFactory factory,
            IAddMessagesToTransportSettings settings,
            ISystemClock systemClock,
            IAddMessagesToTransportRepository repository,
            IOutboxTransport transport,
            IAddMessagesToTransportLogger logger)
        {
            _eventHookPublisher = eventHookPublisher;
            _factory = factory;
            _settings = settings;
            _clock = systemClock;
            _repository = repository;
            _transport = transport;
            _logger = logger;
        }

        public async Task HandleAsync(IHookExecutionContext context, CancellationToken cancellationToken)
        {
            var maxBatchSize = _settings.MaxBatchSize;

            if (context.IsError)
            {
                maxBatchSize = ErrorBatchSize;
            }

            long iteration = 1;
            var numberOfMessages = 0;

            do
            {
                if (iteration > 1)
                {
                    maxBatchSize = _settings.MaxBatchSize;
                }

                numberOfMessages = await _repository.MarkMessages(context.Id, context.Name, maxBatchSize, _clock.TimeProvider, _settings.LockTimeout).ConfigureAwait(false);

                if (numberOfMessages == 0)
                {
                    return;
                }

                var messages = await _repository.GetMarkedMessages(context.Id).ConfigureAwait(false);

                var transportEnvelopes = await _factory.Create(messages).ConfigureAwait(false);

                foreach (var transportEnvelope in transportEnvelopes)
                {
                    await _transport.Add(transportEnvelope).ConfigureAwait(false);
                }

                await _repository.MarkAsProcessed(context.Id, _clock.UtcNow).ConfigureAwait(false);

                _logger.Added(context.Name, context.Id, iteration, numberOfMessages, maxBatchSize);

                await _eventHookPublisher.PublishAsync<AddedMessagesToTransport>().ConfigureAwait(false);

                iteration++;
            }
            while (!cancellationToken.IsCancellationRequested && numberOfMessages >= maxBatchSize);
        }
    }
}
