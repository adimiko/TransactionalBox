using Microsoft.EntityFrameworkCore;
using TransactionalBox.InboxBase.StorageModel.EntityFramework.Internals;

namespace TransactionalBox.InboxBase.StorageModel.EntityFramework
{
    public static class Extensions
    {
        public static void AddInbox(this ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new InboxMessageEntityTypeConfiguration());
        }
    }
}
