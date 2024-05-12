using TransactionalBox.Base.EventHooks;
using TransactionalBox.Inbox.Internals.Hooks.Events;
using TransactionalBox.Inbox.Internals.Storage;

namespace TransactionalBox.Inbox.Internals.Hooks.Handlers.CleanUpInbox
{
    internal sealed class CleanUpInbox : IEventHookHandler<ProcessedMessageFromInbox>
    {
        private readonly IInboxWorkerStorage _storage;

        private readonly ICleanUpInboxSettings _settings;

        public CleanUpInbox(
            IInboxWorkerStorage storage,
            ICleanUpInboxSettings settings)
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
