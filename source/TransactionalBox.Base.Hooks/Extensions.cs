using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Base.Hooks.Internals;
using TransactionalBox.Base.Hooks.Internals.Loggers;

namespace TransactionalBox.Base.Hooks
{
    internal static class Extensions
    {
        internal static void AddEventHookHandler<THookListener, THook>(this IServiceCollection services)
            where THookListener : class, IEventHookHandler<THook>
            where THook : EventHook, new()
        {
            services.AddSingleton<HookHub<THook>>();

            services.AddSingleton<IInternalHookListenersLauncher, HookListenersLauncher<THook>>();

            services.AddSingleton<IEventHookPublisher, EventHookPublisher>();

            services.AddScoped<IEventHookHandler<THook>, THookListener>();

            services.AddSingleton(typeof(IHookListnerLogger<THook>), typeof(HookListnerLogger<THook>));

            services.AddHostedService<Startup>();
        }

    }
}
