using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Base.Hooks.Internals.Contexts;
using TransactionalBox.Base.Hooks.Internals.Loggers;

namespace TransactionalBox.Base.Hooks.Internals
{
    internal sealed class HookListenersLauncher<THook> : IInternalHookListenersLauncher
        where THook : EventHook, new()
    {
        private readonly HookHub<THook> _hookHub;

        private readonly IServiceScopeFactory _serviceScopeFactory;

        private readonly IHookListnerLogger<THook> _logger;

        public HookListenersLauncher(
            HookHub<THook> hookHub,
            IServiceScopeFactory serviceScopeFactory,
            IHookListnerLogger<THook> logger) 
        {
            _hookHub = hookHub;
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        public async Task LaunchAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await foreach (var lastOccurredUtc in _hookHub.ListenAsync(cancellationToken).ConfigureAwait(false))
                    {
                        using (var scope = _serviceScopeFactory.CreateScope())
                        {
                            var eventHookHandler = scope.ServiceProvider.GetRequiredService<IEventHookHandler<THook>>();

                            var id = Guid.NewGuid();
                            var name = eventHookHandler.GetType().Name;

                            var context = new HookExecutionContext(id, name, lastOccurredUtc);

                            _logger.Started(context.Name, context.Id);

                            await eventHookHandler.HandleAsync(context, cancellationToken).ConfigureAwait(false);

                            _logger.Ended(context.Id);
                        }
                    }
                }
                catch (Exception exception) 
                {
                    _logger.UnexpectedError(exception);
                }
            }
        }
    }
}
