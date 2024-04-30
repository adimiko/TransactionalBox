using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using TransactionalBox.Base.BackgroundService.Internals;
using TransactionalBox.Inbox.Internals.Contracts;
using TransactionalBox.Inbox.Internals.Decompression;
using TransactionalBox.Inbox.Internals.Storage;
using TransactionalBox.Inbox.Internals.Topics;
using TransactionalBox.Internals;

namespace TransactionalBox.Inbox.Internals.Jobs
{
    internal sealed class AddMessagesToInboxStorage : Job
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly IDecompressionAlgorithm _decompressionAlgorithm;

        private readonly IInboxWorkerTransport _inboxWorkerTransport;

        private readonly ISystemClock _systemClock;

        private readonly ITopicsProvider _topicsProvider;

        private readonly IAddMessagesToInboxStorageJobSettings _settings;

        public AddMessagesToInboxStorage(
            IServiceProvider serviceProvider,
            IDecompressionAlgorithm decompressionAlgorithm,
            IInboxWorkerTransport inboxWorkerTransport,
            ISystemClock systemClock,
            ITopicsProvider topicsProvider,
            IAddMessagesToInboxStorageJobSettings settings)
        {
            _serviceProvider = serviceProvider;
            _decompressionAlgorithm = decompressionAlgorithm;
            _inboxWorkerTransport = inboxWorkerTransport;
            _systemClock = systemClock;
            _topicsProvider = topicsProvider;
            _settings = settings;
        }

        protected override async Task Execute(CancellationToken stoppingToken)
        {
            await foreach (var messagesFromTransport in _inboxWorkerTransport.GetMessages(_topicsProvider.Topics, stoppingToken))
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var decompressedMessagesFromTransport = await _decompressionAlgorithm.Decompress(messagesFromTransport);
                    //TODO #27
                    var inboxMessages = JsonSerializer.Deserialize<IEnumerable<InboxMessageStorage>>(decompressedMessagesFromTransport);

                    var inboxStorage = scope.ServiceProvider.GetRequiredService<IInboxWorkerStorage>();

                    var existIdempotentInboxKeys = await inboxStorage.GetExistIdempotentInboxKeysBasedOn(inboxMessages);

                    if (!existIdempotentInboxKeys.Any())
                    {
                        //TODO result with duplicated messages and log id in inbox-Worker

                        var idempotentMessages = inboxMessages.Select(x => new IdempotentInboxKey(x.Id, _settings.DefaultTimeToLiveIdempotencyKey, _systemClock.TimeProvider));

                        var result = await inboxStorage.AddRange(inboxMessages, idempotentMessages);

                        if (result == AddRangeToInboxStorageResult.Success) // result.IsSuccess
                        {
                            return;
                        }
                    }

                    AddRangeToInboxStorageResult result1;
                    //int numberOfInboxMessages = inboxMessages.Count(); maxRetry then throw error

                    do
                    {
                        var duplicatedInboxKeys = new List<DuplicatedInboxKey>();

                        existIdempotentInboxKeys = await inboxStorage.GetExistIdempotentInboxKeysBasedOn(inboxMessages);

                        var existIds = existIdempotentInboxKeys.Select(x => x.Id);

                        var duplicatedIds = inboxMessages.Where(x => existIds.Contains(x.Id)).Select(x => x.Id);

                        //TODO log duplicatedIds as Warning

                        var inboxMessagesToSave = inboxMessages.Where(x => existIds.Contains(x.Id));

                        var idempotentMessagesToSave = inboxMessagesToSave.Select(x => new IdempotentInboxKey(x.Id, _settings.DefaultTimeToLiveIdempotencyKey, _systemClock.TimeProvider));

                        result1 = await inboxStorage.AddRange(inboxMessages, idempotentMessagesToSave);
                    }
                    while (result1 == AddRangeToInboxStorageResult.Success);
                }
            }
        }
    }
}
