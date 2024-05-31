using TransactionalBox.Internals.EventHooks;
using TransactionalBox.Internals.EventHooks.Contexts;
using TransactionalBox.Outbox.Internals.Hooks.Events;
using TransactionalBox.Outbox.Internals.Hooks.Handlers.CleanUpOutbox.Logger;
using TransactionalBox.Outbox.Internals.Storage;

namespace TransactionalBox.Outbox.Internals.Hooks.Handlers.CleanUpOutbox
{
    internal sealed class CleanUpOutbox : IEventHookHandler<AddedMessagesToTransport>
    {
        private readonly ICleanUpOutboxRepository _repository;

        private readonly ICleanUpOutboxSettings _settings;

        private readonly ICleanUpOutboxLogger _logger;

        public CleanUpOutbox(
            ICleanUpOutboxRepository repository,
            ICleanUpOutboxSettings settings,
            ICleanUpOutboxLogger logger)
        {
            _repository = repository;
            _settings = settings;
            _logger = logger;
        }

        public async Task HandleAsync(IHookExecutionContext context, CancellationToken cancellationToken)
        {
            long iteration = 1;
            int numberOfRemovedMessages = 0;

            do
            {
                numberOfRemovedMessages = await _repository.RemoveProcessedMessages(_settings.MaxBatchSize).ConfigureAwait(false);

                _logger.CleanedUp(context.Name, context.Id, iteration, numberOfRemovedMessages);

                iteration++;
            }
            while (!cancellationToken.IsCancellationRequested && numberOfRemovedMessages >= _settings.MaxBatchSize);
        }
    }
}
