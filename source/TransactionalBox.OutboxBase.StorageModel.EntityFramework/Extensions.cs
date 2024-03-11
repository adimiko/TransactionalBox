using Microsoft.EntityFrameworkCore;
using TransactionalBox.OutboxBase.StorageModel.EntityFramework.Internals;

namespace TransactionalBox.OutboxBase.StorageModel.EntityFramework
{
    public static class Extensions
    {
        public static void AddOutbox(this ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OutboxMessageEntityTypeConfiguration());
        }
    }
}
