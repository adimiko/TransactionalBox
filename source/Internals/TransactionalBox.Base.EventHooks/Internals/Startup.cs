using Microsoft.Extensions.Hosting;

namespace TransactionalBox.Base.EventHooks.Internals
{
    internal sealed class Startup : BackgroundService
    {
        private readonly IEnumerable<IInternalHookListenersLauncher> _hookListenersLauncher;

        public Startup(
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
