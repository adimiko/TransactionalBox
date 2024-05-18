using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TransactionalBox.Internals.DistributedLock.EntityFrameworkCore.Internals
{
    internal sealed class LockEntityTypeConfiguration<T> : IEntityTypeConfiguration<T>
        where T : Lock
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(x => x.Key);
            builder.Property(x => x.ExpirationUtc);
        }
    }
}
