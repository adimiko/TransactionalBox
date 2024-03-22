using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransactionalBox.OutboxBase.StorageModel.Internals;

namespace TransactionalBox.OutboxBase.StorageModel.EntityFramework.Internals
{
    internal sealed class OutboxLockEntityTypeConfiguration : IEntityTypeConfiguration<OutboxLock>
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
