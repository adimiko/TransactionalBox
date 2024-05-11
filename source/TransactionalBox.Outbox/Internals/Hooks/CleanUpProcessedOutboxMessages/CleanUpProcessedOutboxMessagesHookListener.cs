using TransactionalBox.Base.Hooks;
using TransactionalBox.Outbox.Internals.Storage;

namespace TransactionalBox.Outbox.Internals.Hooks.CleanUpProcessedOutboxMessages
{
    internal sealed class CleanUpProcessedOutboxMessagesHookListener : IHookListener<CleanUpProcessedOutboxMessagesHook>
    {
        private readonly IOutboxWorkerStorage _storage;

        private readonly ICleanUpProcessedOutboxMessagesHookSettings _settings;

        public CleanUpProcessedOutboxMessagesHookListener(
            IOutboxWorkerStorage storage,
            ICleanUpProcessedOutboxMessagesHookSettings settings) 
        {
            _storage = storage;
            _settings = settings;
        }

        public async Task ListenAsync(IHookExecutionContext context, CancellationToken cancellationToken)
        {
            if(!_settings.IsEnabled) 
            {
                return;
            }

            await _storage.RemoveProcessedMessages(_settings.BatchSize).ConfigureAwait(false);

            //TODO when is Equal batch size repeat
        }
    }
}
