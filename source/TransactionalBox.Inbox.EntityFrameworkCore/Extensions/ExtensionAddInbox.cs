using Microsoft.EntityFrameworkCore;
using TransactionalBox.DistributedLock.EntityFramework;
using TransactionalBox.Inbox.Internals.Storage;
using TransactionalBox.Inbox.EntityFrameworkCore.Internals.EntityTypeConfigurations;

namespace TransactionalBox.Inbox
{
    public static class ExtensionAddInbox
    {
        public static void AddInbox(this ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new InboxMessageEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new IdempotentInboxKeyEntityTypeConfiguration());
            modelBuilder.AddDistributedLock<InboxDistributedLock>();
        }
    }
}
