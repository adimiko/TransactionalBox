﻿using TransactionalBox.Internals.Inbox.Hooks.Handlers.CleanUpInbox.Logger;
using TransactionalBox.Internals.Inbox.Storage.ContractsToImplement;
using TransactionalBox.Internals.Inbox.Hooks.Events;
using TransactionalBox.Internals.InternalPackages.EventHooks;

namespace TransactionalBox.Internals.Inbox.Hooks.Handlers.CleanUpInbox
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
                numberOfRemovedMessages = await _storage.RemoveProcessedMessages(_settings.MaxBatchSize).ConfigureAwait(false);

                _logger.CleanedUp(context.Name, context.Id, iteration, numberOfRemovedMessages);

                iteration++;
            }
            while (numberOfRemovedMessages >= _settings.MaxBatchSize);
        }
    }
}
