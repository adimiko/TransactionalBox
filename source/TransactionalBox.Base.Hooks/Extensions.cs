using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Base.Hooks.Internals;

namespace TransactionalBox.Base.Hooks
{
    internal static class Extensions
    {
        internal static void AddHookListener<THookListener, THook>(this IServiceCollection services)
            where THookListener : class, IHookListener<THook>
            where THook : Hook, new()
        {
            services.AddSingleton<HookHub<THook>>();

            services.AddSingleton<IInternalHookListenersLauncher, HookListenersLauncher<THook>>();

            services.AddSingleton<IHookCaller<THook>>(sp => sp.GetRequiredService<HookHub<THook>>());

            services.AddScoped<IHookListener<THook>, THookListener>();

            services.AddHostedService<Startup>();
        }

    }
}
