using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransactionalBox.Outbox.Internals.Storage;

namespace TransactionalBox.Outbox.Storage.EntityFramework.Internals.EntityTypeConfigurations
{
    internal sealed class OutboxMessageEntityTypeConfiguration : IEntityTypeConfiguration<OutboxMessageStorage>
    {
        public void Configure(EntityTypeBuilder<OutboxMessageStorage> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.OccurredUtc).IsRequired();
            builder.Property(x => x.Topic).IsRequired();
            builder.Property(x => x.Payload).IsRequired();
            builder.Property(x => x.LockUtc);
            builder.Property(x => x.IsProcessed).IsConcurrencyToken();
            builder.Property(x => x.JobId).HasMaxLength(254); //TODO
        }
    }
}
