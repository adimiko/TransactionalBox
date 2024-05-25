using Microsoft.EntityFrameworkCore;
using TransactionalBox.Outbox.Internals.Storage;
using TransactionalBox.Outbox.EntityFrameworkCore.Internals.EntityTypeConfigurations;

namespace TransactionalBox
{
    public static class ExtensionAddOutbox
    {
        public static void AddOutbox(this ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OutboxMessageEntityTypeConfiguration());
            modelBuilder.AddDistributedLock<OutboxDistributedLock>();
        }
    }
}
