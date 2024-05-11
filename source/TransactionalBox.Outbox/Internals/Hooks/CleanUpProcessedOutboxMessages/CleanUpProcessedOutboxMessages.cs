using TransactionalBox.Base.EventHooks;
using TransactionalBox.Outbox.Internals.Hooks.EventHooks;
using TransactionalBox.Outbox.Internals.Storage;

namespace TransactionalBox.Outbox.Internals.Hooks.CleanUpProcessedOutboxMessages
{
    internal sealed class CleanUpProcessedOutboxMessages : IEventHookHandler<AddedMessagesToTransportEventHook>
    {
        private readonly IOutboxWorkerStorage _storage;

        private readonly ICleanUpProcessedOutboxMessagesHookSettings _settings;

        public CleanUpProcessedOutboxMessages(
            IOutboxWorkerStorage storage,
            ICleanUpProcessedOutboxMessagesHookSettings settings) 
        {
            _storage = storage;
            _settings = settings;
        }

        public async Task HandleAsync(IHookExecutionContext context, CancellationToken cancellationToken)
        {

            await _storage.RemoveProcessedMessages(_settings.BatchSize).ConfigureAwait(false);

            //TODO when is Equal batch size repeat
        }
    }
}
