using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json;
using TransactionalBox.Base.EventHooks;
using TransactionalBox.Inbox.Internals.Decompression;
using TransactionalBox.Inbox.Internals.Hooks;
using TransactionalBox.Inbox.Internals.Storage;
using TransactionalBox.Inbox.Internals.Transport;
using TransactionalBox.Inbox.Internals.Transport.Topics;
using TransactionalBox.Internals;

namespace TransactionalBox.Inbox.Internals.BackgroundProcesses
{
    internal sealed class AddMessagesToInboxStorage : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly IDecompressionAlgorithm _decompressionAlgorithm;

        private readonly IInboxWorkerTransport _inboxWorkerTransport;

        private readonly ISystemClock _systemClock;

        private readonly ITopicsProvider _topicsProvider;

        private readonly IAddMessagesToInboxStorageJobSettings _settings;

        private readonly IEventHookPublisher _eventHookPublisher;

        public AddMessagesToInboxStorage(
            IServiceProvider serviceProvider,
            IDecompressionAlgorithm decompressionAlgorithm,
            IInboxWorkerTransport inboxWorkerTransport,
            ISystemClock systemClock,
            ITopicsProvider topicsProvider,
            IAddMessagesToInboxStorageJobSettings settings,
            IEventHookPublisher eventHookPublisher)
        {
            _serviceProvider = serviceProvider;
            _decompressionAlgorithm = decompressionAlgorithm;
            _inboxWorkerTransport = inboxWorkerTransport;
            _systemClock = systemClock;
            _topicsProvider = topicsProvider;
            _settings = settings;
            _eventHookPublisher = eventHookPublisher;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await foreach (var messagesFromTransport in _inboxWorkerTransport.GetMessages(_topicsProvider.Topics, stoppingToken).ConfigureAwait(false))
                    {
                        using (var scope = _serviceProvider.CreateScope())
                        {
                            var decompressedMessagesFromTransport = await _decompressionAlgorithm.Decompress(messagesFromTransport).ConfigureAwait(false);
                            //TODO #27
                            var inboxMessages = JsonSerializer.Deserialize<IEnumerable<InboxMessageStorage>>(decompressedMessagesFromTransport);

                            var inboxStorage = scope.ServiceProvider.GetRequiredService<IInboxWorkerStorage>();

                            var existIdempotentInboxKeys = await inboxStorage.GetExistIdempotentInboxKeysBasedOn(inboxMessages).ConfigureAwait(false);

                            if (!existIdempotentInboxKeys.Any())
                            {
                                //TODO result with duplicated messages and log id in inbox-Worker

                                var idempotentMessages = inboxMessages.Select(x => new IdempotentInboxKey(x.Id, _settings.DefaultTimeToLiveIdempotencyKey, _systemClock.TimeProvider));

                                var result = await inboxStorage.AddRange(inboxMessages, idempotentMessages).ConfigureAwait(false);

                                if (result == AddRangeToInboxStorageResult.Success) // result.IsSuccess
                                {
                                    await _eventHookPublisher.PublishAsync<AddedMessagesToInboxEventHook>().ConfigureAwait(false);

                                    return;
                                }
                            }

                            AddRangeToInboxStorageResult result1;
                            //int numberOfInboxMessages = inboxMessages.Count(); maxRetry then throw error

                            do
                            {
                                var duplicatedInboxKeys = new List<DuplicatedInboxKey>();

                                existIdempotentInboxKeys = await inboxStorage.GetExistIdempotentInboxKeysBasedOn(inboxMessages).ConfigureAwait(false);

                                var existIds = existIdempotentInboxKeys.Select(x => x.Id);

                                var duplicatedIds = inboxMessages.Where(x => existIds.Contains(x.Id)).Select(x => x.Id);

                                //TODO log duplicatedIds as Warning

                                var inboxMessagesToSave = inboxMessages.Where(x => existIds.Contains(x.Id));

                                var idempotentMessagesToSave = inboxMessagesToSave.Select(x => new IdempotentInboxKey(x.Id, _settings.DefaultTimeToLiveIdempotencyKey, _systemClock.TimeProvider));

                                result1 = await inboxStorage.AddRange(inboxMessages, idempotentMessagesToSave);
                            }
                            while (result1 == AddRangeToInboxStorageResult.Success);

                            await _eventHookPublisher.PublishAsync<AddedMessagesToInboxEventHook>().ConfigureAwait(false);
                        }
                    }
                }
                catch (Exception)
                {
                    //TODO
                    await Task.Delay(500, stoppingToken);
                    //TODO logger
                }
            }
        }
    }
}
