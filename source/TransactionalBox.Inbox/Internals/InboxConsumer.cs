using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TransactionalBox.Inbox.Internals
{
    internal sealed class InboxConsumer : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public InboxConsumer(IServiceProvider serviceProvider) 
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested) 
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    // get message

                    //var inboxHandler = scope.ServiceProvider.GetRequiredService<IInbox<>>();

                    // save changes ?
                }
            }

            await Task.CompletedTask;
        }
    }
}
