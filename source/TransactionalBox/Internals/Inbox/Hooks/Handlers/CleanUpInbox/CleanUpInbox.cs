using TransactionalBox.Internals.Inbox.Hooks.Handlers.CleanUpInbox.Logger;
using TransactionalBox.Internals.Inbox.Storage.ContractsToImplement;
using TransactionalBox.Internals.Inbox.Hooks.Events;
using TransactionalBox.Internals.InternalPackages.EventHooks;

namespace TransactionalBox.Internals.Inbox.Hooks.Handlers.CleanUpInbox
{
    internal sealed class CleanUpInbox : IEventHookHandler<ProcessedMessageFromInbox>
    {
        private readonly ICleanUpInboxRepository _repo;

        private readonly ICleanUpInboxSettings _settings;

        private readonly ICleanUpInboxLogger _logger;

        public CleanUpInbox(
            ICleanUpInboxRepository repo,
            ICleanUpInboxSettings settings,
            ICleanUpInboxLogger logger)
        {
            _repo = repo;
            _settings = settings;
            _logger = logger;
        }

        public async Task HandleAsync(IHookExecutionContext context, CancellationToken cancellationToken)
        {
            long iteration = 1;
            int numberOfRemovedMessages = 0;

            do
            {
                numberOfRemovedMessages = await _repo.RemoveProcessedMessages(_settings.MaxBatchSize).ConfigureAwait(false);

                _logger.CleanedUp(context.Name, context.Id, iteration, numberOfRemovedMessages);

                iteration++;
            }
            while (numberOfRemovedMessages >= _settings.MaxBatchSize);
        }
    }
}
