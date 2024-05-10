using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Base.Hooks.Internals;

namespace TransactionalBox.Base.Hooks
{
    internal static class Extensions
    {
        internal static void AddHook<THook>(this IServiceCollection services)
            where THook : Hook
        {
            services.AddSingleton<HookHub<THook>>();
            services.AddSingleton<THook>();
            services.AddSingleton<Hook, THook>();
            services.AddSingleton<IHookCaller<THook>>(sp => sp.GetRequiredService<HookHub<THook>>());
            services.AddSingleton<IHookListener<THook>>(sp => sp.GetRequiredService<HookHub<THook>>());

            services.AddHostedService<HookService>();
        }

    }
}
