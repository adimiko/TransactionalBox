using Microsoft.Extensions.Hosting;

namespace TransactionalBox.Internals.EventHooks.Internals
{
    internal sealed class EventHooksStartup : BackgroundService
    {
        private readonly IEnumerable<IInternalHookListenersLauncher> _hookListenersLauncher;

        public EventHooksStartup(
            IEnumerable<IInternalHookListenersLauncher> hookListenersLauncher) 
        {
            _hookListenersLauncher = hookListenersLauncher;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            foreach (var hookListenersLauncher in _hookListenersLauncher) 
            {
                hookListenersLauncher.LaunchAsync(stoppingToken);
            }

            return Task.CompletedTask;
        }
    }
}
