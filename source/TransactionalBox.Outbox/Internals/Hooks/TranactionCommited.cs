using TransactionalBox.Base.Hooks;

namespace TransactionalBox.Outbox.Internals.Hooks
{
    internal class TranactionCommited : ITranactionCommited
    {
        private readonly IHookCaller<AddMessagesToTransportHook> _hookCaller;
        public TranactionCommited(IHookCaller<AddMessagesToTransportHook> hookCaller)
        {
            _hookCaller = hookCaller;
        }

        public async Task Commited()
        {
            await _hookCaller.CallAsync();
            //TODO observability
        }
    }
}
