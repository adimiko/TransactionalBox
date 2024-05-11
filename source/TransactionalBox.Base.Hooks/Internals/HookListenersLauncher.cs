using Microsoft.Extensions.DependencyInjection;

namespace TransactionalBox.Base.Hooks.Internals
{
    internal sealed class HookListenersLauncher<T> : IInternalHookListenersLauncher
        where T : Hook, new()
    {
        private readonly HookHub<T> _hookHub;

        private readonly IServiceScopeFactory _serviceScopeFactory;

        public HookListenersLauncher(
            HookHub<T> hookHub,
            IServiceScopeFactory serviceScopeFactory) 
        {
            _hookHub = hookHub;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task LaunchAsync(CancellationToken cancellationToken)
        {
            await foreach(var lastHook in _hookHub.ListenAsync(cancellationToken).ConfigureAwait(false)) 
            {
                //TODO exception handling
                using (var scope = _serviceScopeFactory.CreateScope()) 
                {
                    var hookListner = scope.ServiceProvider.GetRequiredService<IHookListener<T>>();

                    await hookListner.ListenAsync(lastHook, cancellationToken).ConfigureAwait(false);
                }
            }
        }
    }
}
