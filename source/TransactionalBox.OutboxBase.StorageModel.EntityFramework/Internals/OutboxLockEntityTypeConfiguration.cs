using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TransactionalBox.OutboxBase.StorageModel.EntityFramework.Internals
{
    internal class OutboxLockEntityTypeConfiguration : IEntityTypeConfiguration<OutboxLock>
    {
        public void Configure(EntityTypeBuilder<OutboxLock> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ExpirationUtc);
            builder.Property(x => x.IsReleased);
        }
    }
}
