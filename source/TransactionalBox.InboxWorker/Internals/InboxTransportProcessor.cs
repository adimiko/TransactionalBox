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
            var x = _systemClock.UtcNow;
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
                            await inboxStorage.AddRange(inboxMessages, _systemClock.UtcNow);
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
