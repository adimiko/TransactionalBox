using Microsoft.Extensions.Hosting;

namespace TransactionalBox.Base.Hooks.Internals
{
    internal sealed class HookService : BackgroundService
    {
        private readonly IEnumerable<Hook> _hooks;

        public HookService(IEnumerable<Hook> hooks) 
        {
            _hooks = hooks;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            foreach (Hook hook in _hooks) 
            {
                hook.StartAsync(stoppingToken);
            }

            return Task.CompletedTask;
        }
    }
}
