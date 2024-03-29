using Microsoft.EntityFrameworkCore;
using TransactionalBox.DistributedLock.EntityFramework;
using TransactionalBox.InboxBase.StorageModel.Internals;

namespace TransactionalBox.InboxBase.StorageModel.EntityFramework.Internals
{
    internal static class Extensions
    {
        internal static void AddInboxStorageModel(this ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new InboxMessageEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new IdempotentInboxKeyEntityTypeConfiguration());
            modelBuilder.AddDistributedLock<InboxDistributedLock>();
        }
    }
}
