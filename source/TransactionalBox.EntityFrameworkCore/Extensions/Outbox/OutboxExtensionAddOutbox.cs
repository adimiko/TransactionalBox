using Microsoft.EntityFrameworkCore;
using TransactionalBox.Outbox.Internals.Storage;
using TransactionalBox.EntityFrameworkCore.Internals.Outbox.EntityTypeConfigurations;

namespace TransactionalBox
{
    public static class OutboxExtensionAddOutbox
    {
        public static void AddOutbox(this ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OutboxMessageEntityTypeConfiguration());
            modelBuilder.AddDistributedLock<OutboxDistributedLock>();
        }
    }
}
