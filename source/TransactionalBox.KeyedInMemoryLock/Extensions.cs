using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.KeyedInMemoryLock.Internals;

namespace TransactionalBox.KeyedInMemoryLock
{
    public static class Extensions
    {
        public static void AddKeyedInMemoryLock(this IServiceCollection services)
        {
            services.AddScoped<IKeyedInMemoryLock, InternalKeyedInMemoryLock>();
        }
    }
}
