using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TransactionalBox.InboxBase.StorageModel;
using TransactionalBox.Internals;

namespace TransactionalBox.InboxWorker.Internals
{
    internal sealed class InboxTransportProcessor : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly IInboxWorkerTransport _inboxWorkerTransport;

        private readonly ISystemClock _systemClock;

        public InboxTransportProcessor(
            IInboxWorkerTransport inboxWorkerTransport,
            IServiceProvider serviceProvider,
            ISystemClock systemClock) 
        {
            _inboxWorkerTransport = inboxWorkerTransport;
            _serviceProvider = serviceProvider;
            _systemClock = systemClock;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await foreach (var inboxMessages in _inboxWorkerTransport.GetMessages(stoppingToken))
                    {
                        using (var scope = _serviceProvider.CreateScope())
                        {
                            //TODO check in storage does message exist
                            var inboxStorage = scope.ServiceProvider.GetRequiredService<IInboxStorage>();

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
                catch(Exception ex)
                {
                    //log
                    await Task.Delay(500);
                }
            }
        }
    }
}
