using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TransactionalBox.InboxBase.StorageModel;

namespace TransactionalBox.InboxWorker.Internals
{
    internal sealed class InboxTransportProcessor : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly IInboxWorkerTransport _inboxWorkerTransport;

        public InboxTransportProcessor(
            IInboxWorkerTransport inboxWorkerTransport,
            IServiceProvider serviceProvider) 
        {
            _inboxWorkerTransport = inboxWorkerTransport;
            _serviceProvider = serviceProvider;
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
                            await scope.ServiceProvider.GetRequiredService<IInboxStorage>().AddRange(inboxMessages);
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
