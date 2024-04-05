using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using TransactionalBox.BackgroundServiceBase.Internals;
using TransactionalBox.InboxBase.StorageModel.Internals;
using TransactionalBox.InboxWorker.Decompression;
using TransactionalBox.InboxWorker.Internals.Contracts;
using TransactionalBox.InboxWorker.Internals.Services;
using TransactionalBox.Internals;

namespace TransactionalBox.InboxWorker.Internals.Jobs
{
    internal sealed class AddMessagesToInboxStorage : Job
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly IDecompressionAlgorithm _decompressionAlgorithm;

        private readonly IInboxWorkerTransport _inboxWorkerTransport;

        private readonly ISystemClock _systemClock;

        private readonly ITopicsProvider _topicsProvider;

        public AddMessagesToInboxStorage(
            IServiceProvider serviceProvider,
            IDecompressionAlgorithm decompressionAlgorithm,
            IInboxWorkerTransport inboxWorkerTransport,
            ISystemClock systemClock,
            ITopicsProvider topicsProvider) 
        {
            _serviceProvider = serviceProvider;
            _decompressionAlgorithm = decompressionAlgorithm;
            _inboxWorkerTransport = inboxWorkerTransport;
            _systemClock = systemClock;
            _topicsProvider = topicsProvider;
        }

        protected override async Task Execute(CancellationToken stoppingToken)
        {
            await foreach (var messagesFromTransport in _inboxWorkerTransport.GetMessages(_topicsProvider.Topics, stoppingToken))
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var decompressedMessagesFromTransport = await _decompressionAlgorithm.Decompress(messagesFromTransport);
                    //TODO #27
                    var inboxMessages = JsonSerializer.Deserialize<IEnumerable<InboxMessage>>(decompressedMessagesFromTransport);

                    var inboxStorage = scope.ServiceProvider.GetRequiredService<IInboxWorkerStorage>();

                    var existIdempotentInboxKeys = await inboxStorage.GetExistIdempotentInboxKeysBasedOn(inboxMessages);

                    if (!existIdempotentInboxKeys.Any())
                    {
                        var result = await inboxStorage.AddRange(inboxMessages, _systemClock.UtcNow);

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

                        result1 = await inboxStorage.AddRange(inboxMessages, _systemClock.UtcNow);
                    }
                    while (result1 == AddRangeToInboxStorageResult.Success);
                }
            }
        }
    }
}
