using Microsoft.EntityFrameworkCore;

namespace TransactionalBox.OutboxBase.StorageModel.EntityFramework.Internals
{
    internal static class Extensions
    {
        internal static void AddOutboxStorageModel(this ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OutboxMessageEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OutboxLockEntityTypeConfiguration());
        }
    }
}
