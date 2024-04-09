using Microsoft.EntityFrameworkCore;
using TransactionalBox.DistributedLock.EntityFramework;
using TransactionalBox.Base.Inbox.StorageModel.Internals;

namespace TransactionalBox.Base.Inbox.StorageModel.EntityFramework.Internals
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
