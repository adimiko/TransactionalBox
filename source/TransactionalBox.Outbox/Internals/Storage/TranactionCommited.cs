using TransactionalBox.Internals.EventHooks;
using TransactionalBox.Outbox.Internals.Hooks.Events;

namespace TransactionalBox.Outbox.Internals.Storage
{
    internal class TranactionCommited : ITranactionCommited
    {
        private readonly IEventHookPublisher _eventHookPublisher;
        public TranactionCommited(
            IEventHookPublisher eventHookPublisher)
        {
            _eventHookPublisher = eventHookPublisher;
        }

        public async Task Commited()
        {
            await _eventHookPublisher.PublishAsync<AddedMessagesToOutbox>().ConfigureAwait(false);
            //TODO observability
        }
    }
}
