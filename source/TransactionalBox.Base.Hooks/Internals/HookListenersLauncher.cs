using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Base.Hooks.Internals.Contexts;
using TransactionalBox.Base.Hooks.Internals.Loggers;

namespace TransactionalBox.Base.Hooks.Internals
{
    internal sealed class HookListenersLauncher<T> : IInternalHookListenersLauncher
        where T : Hook, new()
    {
        private readonly HookHub<T> _hookHub;

        private readonly IServiceScopeFactory _serviceScopeFactory;

        private readonly IHookListnerLogger _logger;

        public HookListenersLauncher(
            HookHub<T> hookHub,
            IServiceScopeFactory serviceScopeFactory,
            IHookListnerLogger logger) 
        {
            _hookHub = hookHub;
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        public async Task LaunchAsync(CancellationToken cancellationToken)
        {
            await foreach(var lastOccurredUtc in _hookHub.ListenAsync(cancellationToken).ConfigureAwait(false)) 
            {
                //TODO exception handling
                using (var scope = _serviceScopeFactory.CreateScope()) 
                {
                    var hookListner = scope.ServiceProvider.GetRequiredService<IHookListener<T>>();

                    var id = Guid.NewGuid();
                    var name = typeof(T).Name;

                    var context = new HookExecutionContext(id, name, lastOccurredUtc);

                    _logger.Started(context.Name, context.Id);

                    await hookListner.ListenAsync(context, cancellationToken).ConfigureAwait(false);

                    _logger.Ended(context.Id);
                }
            }
        }
    }
}
