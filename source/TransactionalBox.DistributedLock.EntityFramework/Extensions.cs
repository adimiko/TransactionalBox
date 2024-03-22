using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.DistributedLock.EntityFramework.Internals;

namespace TransactionalBox.DistributedLock.EntityFramework
{
    public static class Extensions
    {
        public static IServiceCollection AddEntityFrameworkDistributedLock<T>(this IServiceCollection services)
            where T : Lock, new()
        {
            services.AddScoped<IDistributedLock<T>, EntityFrameworkDistributedLock<T>>();

            return services;
        }

        public static void AddDistributedLock<T>(this ModelBuilder modelBuilder) 
            where T : Lock, new()
        {
            modelBuilder.ApplyConfiguration(new LockEntityTypeConfiguration<T>());
        }

    }
}
