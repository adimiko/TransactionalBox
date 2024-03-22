using Microsoft.EntityFrameworkCore;

namespace TransactionalBox.InboxBase.StorageModel.EntityFramework.Internals
{
    internal static class Extensions
    {
        internal static void AddInboxStorageModel(this ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new InboxMessageEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new IdempotentInboxKeyEntityTypeConfiguration());
        }
    }
}
