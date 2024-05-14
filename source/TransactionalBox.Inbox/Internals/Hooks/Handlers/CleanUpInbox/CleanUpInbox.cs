using TransactionalBox.Base.EventHooks;
using TransactionalBox.Inbox.Internals.Hooks.Events;
using TransactionalBox.Inbox.Internals.Hooks.Handlers.CleanUpInbox.Logger;
using TransactionalBox.Inbox.Internals.Storage;

namespace TransactionalBox.Inbox.Internals.Hooks.Handlers.CleanUpInbox
{
    internal sealed class CleanUpInbox : IEventHookHandler<ProcessedMessageFromInbox>
    {
        private readonly IInboxWorkerStorage _storage;

        private readonly ICleanUpInboxSettings _settings;

        private readonly ICleanUpInboxLogger _logger;

        public CleanUpInbox(
            IInboxWorkerStorage storage,
            ICleanUpInboxSettings settings,
            ICleanUpInboxLogger logger)
        {
            _storage = storage;
            _settings = settings;
            _logger = logger;
        }

        public async Task HandleAsync(IHookExecutionContext context, CancellationToken cancellationToken)
        {
            long iteration = 1;
            int numberOfRemovedMessages = 0;

            do
            {
                numberOfRemovedMessages = await _storage.RemoveProcessedMessages(_settings.BatchSize).ConfigureAwait(false);

                _logger.CleanedUp(context.Name, context.Id, iteration, numberOfRemovedMessages);

                iteration++;
            }
            while (numberOfRemovedMessages >= _settings.BatchSize);
        }
    }
}
