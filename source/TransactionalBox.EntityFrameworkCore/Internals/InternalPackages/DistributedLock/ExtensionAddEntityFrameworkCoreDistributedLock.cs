using Microsoft.EntityFrameworkCore;
using TransactionalBox.Internals.InternalPackages.DistributedLock;

namespace TransactionalBox.EntityFrameworkCore.Internals.InternalPackages.DistributedLock
{
    internal static class ExtensionAddEntityFrameworkCoreDistributedLock
    {
        internal static void AddEntityFrameworkCoreDistributedLock<T>(this ModelBuilder modelBuilder)
            where T : Lock, new()
        {
            modelBuilder.ApplyConfiguration(new LockEntityTypeConfiguration<T>());
        }
    }
}
