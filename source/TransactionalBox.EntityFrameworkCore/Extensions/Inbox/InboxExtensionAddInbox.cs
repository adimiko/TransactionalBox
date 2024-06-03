using Microsoft.EntityFrameworkCore;
using TransactionalBox.EntityFrameworkCore.Internals.Inbox.EntityTypeConfigurations;
using TransactionalBox.Internals.Inbox.Storage;

namespace TransactionalBox
{
    public static class InboxExtensionAddInbox
    {
        public static void AddInbox(this ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new InboxMessageEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new IdempotentInboxKeyEntityTypeConfiguration());
            modelBuilder.AddDistributedLock<InboxDistributedLock>();
        }
    }
}
