using Microsoft.EntityFrameworkCore;
using TransactionalBox.Outbox.EntityFramework.Internals;

namespace TransactionalBox.Outbox.EntityFramework
{
    public static class ModelBuilderExtensions
    {
        public static void AddOutbox(this ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OutboxMessageEntityTypeConfiguration());
        }
    }
}
