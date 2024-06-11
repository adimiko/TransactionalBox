using Microsoft.Extensions.Hosting;
using TransactionalBox.Internals.InternalPackages.EventHooks;
using TransactionalBox.Internals.Outbox.Hooks.Events;

namespace TransactionalBox.Internals.Outbox
{
    internal sealed class OutboxStartup : BackgroundService
    {
        private readonly IEventHookPublisher _eventHookPublisher;

        public OutboxStartup(IEventHookPublisher eventHookPublisher)
        {
            _eventHookPublisher = eventHookPublisher;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _eventHookPublisher.PublishAsync<AddedMessagesToOutbox>().ConfigureAwait(false);
        }
    }
}
