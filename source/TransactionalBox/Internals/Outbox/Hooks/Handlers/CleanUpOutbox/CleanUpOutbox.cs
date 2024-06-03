using TransactionalBox.Internals.EventHooks;
using TransactionalBox.Internals.EventHooks.Contexts;
using TransactionalBox.Internals.Outbox.Hooks.Events;
using TransactionalBox.Internals.Outbox.Hooks.Handlers.CleanUpOutbox.Logger;
using TransactionalBox.Internals.Outbox.Storage.ContractsToImplement;

namespace TransactionalBox.Internals.Outbox.Hooks.Handlers.CleanUpOutbox
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
