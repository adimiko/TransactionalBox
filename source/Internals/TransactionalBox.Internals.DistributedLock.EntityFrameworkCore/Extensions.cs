using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Internals.DistributedLock.Configurators;
using TransactionalBox.Internals.DistributedLock.EntityFrameworkCore.Internals;
using TransactionalBox.Internals.DistributedLock.Internals.Contracts;

namespace TransactionalBox.Internals.DistributedLock.EntityFrameworkCore
{
    public static class Extensions
    {
        public static IServiceCollection UseEntityFramework<TDbContext>(
            this IDistributedLockStorageConfigurator storageConfigurator)
            where TDbContext : DbContext
        {
            var services = storageConfigurator.Services;

            services.AddScoped<DbContext>(sp => sp.GetRequiredService<TDbContext>());
            services.AddScoped<IDistributedLockStorage, EntityFrameworkDistributedLockStorage>();

            return services;
        }

        public static IServiceCollection UseEntityFramework(
            this IDistributedLockStorageConfigurator storageConfigurator)
        {
            var services = storageConfigurator.Services;

            services.AddScoped<IDistributedLockStorage, EntityFrameworkDistributedLockStorage>();

            return services;
        }

        public static void AddDistributedLock<T>(this ModelBuilder modelBuilder) 
            where T : Lock, new()
        {
            modelBuilder.ApplyConfiguration(new LockEntityTypeConfiguration<T>());
        }

    }
}
