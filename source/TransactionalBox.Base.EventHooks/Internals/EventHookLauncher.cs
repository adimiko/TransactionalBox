﻿using Microsoft.Extensions.DependencyInjection;
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