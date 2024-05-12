using TransactionalBox.Base.EventHooks;
using TransactionalBox.Outbox.Internals.Hooks.Events;
using TransactionalBox.Outbox.Internals.Storage;

namespace TransactionalBox.Outbox.Internals.Hooks.Handlers.CleanUpOutbox
{
    internal sealed class CleanUpOutbox : IEventHookHandler<AddedMessagesToTransport>
    {
        private readonly IOutboxWorkerStorage _storage;

        private readonly ICleanUpOutboxSettings _settings;

        public CleanUpOutbox(
            IOutboxWorkerStorage storage,
            ICleanUpOutboxSettings settings)
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
            while (!cancellationToken.IsCancellationRequested && numberOfRemovedMessages >= _settings.BatchSize);
        }
    }
}
