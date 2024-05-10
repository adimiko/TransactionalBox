using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Base.Hooks;
using TransactionalBox.Internals;
using TransactionalBox.Outbox.Internals.Storage;

namespace TransactionalBox.Outbox.Internals.Hooks.CleanUpProcessedOutboxMessages
{
    internal sealed class CleanUpProcessedOutboxMessagesHook : Hook
    {
        private readonly IHookListener<CleanUpProcessedOutboxMessagesHook> _hookListener;

        private readonly ISystemClock _systemClock;

        private readonly IServiceScopeFactory _serviceScopeFactory;

        private readonly ICleanUpProcessedOutboxMessagesHookSettings _settings;

        public CleanUpProcessedOutboxMessagesHook(
            IHookListener<CleanUpProcessedOutboxMessagesHook> hookListener,
            ISystemClock systemClock,
            IServiceScopeFactory serviceScopeFactory,
            ICleanUpProcessedOutboxMessagesHookSettings settings)
        {
            _hookListener = hookListener;
            _systemClock = systemClock;
            _serviceScopeFactory = serviceScopeFactory;
            _settings = settings;
        }

        protected internal override async Task StartAsync(CancellationToken cancellationToken)
        {
            if (!_settings.IsEnabled)
            {
                return;
            }

            await foreach (var lastHook in _hookListener.ListenAsync(cancellationToken).ConfigureAwait(false))
            {
                await Process(cancellationToken).ConfigureAwait(false);
            }
        }

        private async Task Process(CancellationToken cancellationToken)
        {
            using var scope = _serviceScopeFactory.CreateScope();

            var storage = scope.ServiceProvider.GetRequiredService<IOutboxWorkerStorage>();

            var numberOfRemovedMessages = await storage.RemoveProcessedMessages(_settings.BatchSize);
            
            //TODO when is Equal batch size repeat
        }
    }
}
