using Microsoft.Extensions.DependencyInjection;

namespace TransactionalBox.Internals.InternalPackages.EventHooks
{
    internal sealed class EventHookPublisher : IEventHookPublisher
    {
        private readonly IServiceProvider _serviceProvider;

        public EventHookPublisher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task PublishAsync<TEventHook>()
            where TEventHook : EventHook, new()
        {
            var eventHookHub = _serviceProvider.GetService<EventHookHub<TEventHook>>();

            if (eventHookHub is not null)
            {
                await eventHookHub.PublishAsync().AsTask().ConfigureAwait(false);
            }
        }
    }
}
