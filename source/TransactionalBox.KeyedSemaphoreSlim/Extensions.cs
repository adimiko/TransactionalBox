using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.KeyedSemaphoreSlim.Internals;

namespace TransactionalBox.KeyedSemaphoreSlim
{
    public static class Extensions
    {
        public static void AddKeyedSemaphoreSlim(this IServiceCollection services)
        {
            services.AddScoped<IKeyedSemaphoreSlim, InternalKeyedSemaphoreSlim>();
        }
    }
}
