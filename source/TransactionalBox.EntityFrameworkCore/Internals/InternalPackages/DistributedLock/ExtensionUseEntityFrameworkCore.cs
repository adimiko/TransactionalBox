using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Internals.InternalPackages.DistributedLock;

namespace TransactionalBox.EntityFrameworkCore.Internals.InternalPackages.DistributedLock
{
    internal static class ExtensionUseEntityFrameworkCore
    {
        internal static IServiceCollection UseEntityFrameworkCore<TDbContext>(
            this IDistributedLockStorageConfigurator storageConfigurator)
            where TDbContext : DbContext
        {
            var services = storageConfigurator.Services;

            services.AddScoped<DbContext>(sp => sp.GetRequiredService<TDbContext>());
            services.AddScoped<IDistributedLockStorage, EntityFrameworkDistributedLockStorage>();

            return services;
        }

        internal static IServiceCollection UseEntityFrameworkCore(
            this IDistributedLockStorageConfigurator storageConfigurator)
        {
            var services = storageConfigurator.Services;

            services.AddScoped<IDistributedLockStorage, EntityFrameworkDistributedLockStorage>();

            return services;
        }
    }
}
