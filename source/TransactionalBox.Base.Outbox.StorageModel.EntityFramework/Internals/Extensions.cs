using Microsoft.EntityFrameworkCore;
using TransactionalBox.Base.Outbox.StorageModel.Internals;
using TransactionalBox.DistributedLock.EntityFramework;

namespace TransactionalBox.Base.Outbox.StorageModel.EntityFramework.Internals
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
