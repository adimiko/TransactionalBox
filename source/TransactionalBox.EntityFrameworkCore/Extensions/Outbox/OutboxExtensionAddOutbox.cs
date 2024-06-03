using Microsoft.EntityFrameworkCore;
using TransactionalBox.EntityFrameworkCore.Internals.Outbox.EntityTypeConfigurations;
using TransactionalBox.Internals.Outbox.Storage;

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
