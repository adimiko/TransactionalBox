using Microsoft.Extensions.DependencyInjection;
using System;
using TransactionalBox.Base.EventHooks.Internals.Contexts;
using TransactionalBox.Base.EventHooks.Internals.Loggers;

namespace TransactionalBox.Base.EventHooks.Internals
{
    internal sealed class EventHookLauncher<THook> : IInternalHookListenersLauncher
        where THook : EventHook, new()
    {
        private readonly EventHookHub<THook> _hookHub;

        private readonly IServiceScopeFactory _serviceScopeFactory;

        private readonly IHookListnerLogger<THook> _logger;

        public EventHookLauncher(
            EventHookHub<THook> hookHub,
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

                            var isError = false;
                            long attempt = 0;

                            var context = new HookExecutionContext(id, name, lastOccurredUtc, isError, attempt);

                            _logger.Started(context.Name, context.Id);

                            do
                            {
                                try
                                {
                                    await eventHookHandler.HandleAsync(context, cancellationToken).ConfigureAwait(false);

                                    isError = false;
                                    attempt = 0;
                                }
                                catch (Exception exception)
                                {
                                    isError = true;
                                    attempt++;

                                    context = new HookExecutionContext(id, name, lastOccurredUtc, isError, attempt);

                                    _logger.UnexpectedException(context.Name, context.Id, context.Attempt, exception);
                                }
                            }
                            while (isError);

                            _logger.Ended(context.Name, context.Id);
                        }
                    }
                }
                catch (Exception exception) 
                {
                    _logger.UnexpectedException(exception);
                }
            }
        }
    }
}
