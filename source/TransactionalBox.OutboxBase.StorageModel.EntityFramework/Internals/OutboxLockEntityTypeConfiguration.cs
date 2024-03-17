using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TransactionalBox.OutboxBase.StorageModel.EntityFramework.Internals
{
    internal class OutboxLockEntityTypeConfiguration : IEntityTypeConfiguration<OutboxLock>
    {
        public void Configure(EntityTypeBuilder<OutboxLock> builder)
        {
            builder.HasKey(x => x.Key);
            builder.Property(x => x.StartUtc);
            builder.Property(x => x.TimeoutUtc);
            builder.Property(x => x.ConcurrencyToken);
            builder.Property(x => x.IsReleased);
        }
    }
}
