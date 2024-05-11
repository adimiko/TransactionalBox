using TransactionalBox.Base.Hooks;
using TransactionalBox.Outbox.Internals.Hooks.EventHooks;

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
            await _eventHookPublisher.PublishAsync<AddedMessagesToOutboxEventHook>().ConfigureAwait(false);
            //TODO observability
        }
    }
}
