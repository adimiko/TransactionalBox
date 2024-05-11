using TransactionalBox.Base.BackgroundService.Internals;
using TransactionalBox.Base.EventHooks;
using TransactionalBox.Inbox.Internals.Jobs;
using TransactionalBox.Inbox.Internals.Storage;

namespace TransactionalBox.Inbox.Internals.Hooks
{
    internal sealed class CleanUpProcessedInboxMessages : IEventHookHandler<ProcessedMessageFromInboxEventHook>
    {
        private readonly IInboxWorkerStorage _storage;

        private readonly ICleanUpProcessedInboxMessagesJobSettings _settings;

        public CleanUpProcessedInboxMessages(
            IInboxWorkerStorage storage,
            ICleanUpProcessedInboxMessagesJobSettings settings)
        {
            _storage = storage;
            _settings = settings;
        }

        public async Task HandleAsync(IHookExecutionContext context, CancellationToken cancellationToken)
        {
            int numberOfRemovedMessages = 0;

            do
            {
                numberOfRemovedMessages = await _storage.RemoveProcessedMessages(_settings.BatchSize).ConfigureAwait(false);
            }
            while (numberOfRemovedMessages >= _settings.BatchSize);
        }
    }
}
