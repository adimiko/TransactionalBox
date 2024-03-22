using Microsoft.EntityFrameworkCore;
using TransactionalBox.OutboxBase.StorageModel.Internals;
using TransactionalBox.DistributedLock.EntityFramework;

namespace TransactionalBox.OutboxBase.StorageModel.EntityFramework.Internals
{
    internal static class Extensions
    {
        internal static void AddOutboxStorageModel(this ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OutboxMessageEntityTypeConfiguration());
            modelBuilder.AddDistributedLock<OutboxDistributedLock>();
        }
    }
}
