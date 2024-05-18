using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Internals.EventHooks.Internals;
using TransactionalBox.Internals.EventHooks.Internals.Loggers;

namespace TransactionalBox.Internals.EventHooks
{
    internal static class Extensions
    {
        internal static void AddEventHookHandler<THookListener, THook>(this IServiceCollection services)
            where THookListener : class, IEventHookHandler<THook>
            where THook : EventHook, new()
        {
            services.AddSingleton<EventHookHub<THook>>();

            services.AddSingleton<IInternalHookListenersLauncher, EventHookLauncher<THook>>();

            services.AddSingleton<IEventHookPublisher, EventHookPublisher>();

            services.AddScoped<IEventHookHandler<THook>, THookListener>();

            services.AddSingleton(typeof(IHookListnerLogger<THook>), typeof(HookListnerLogger<THook>));

            services.AddHostedService<Startup>();
        }

    }
}
