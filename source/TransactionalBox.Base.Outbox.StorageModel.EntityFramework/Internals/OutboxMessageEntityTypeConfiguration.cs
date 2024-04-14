using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransactionalBox.Base.Outbox.StorageModel.Internals;

namespace TransactionalBox.Base.Outbox.StorageModel.EntityFramework.Internals
{
    internal sealed class OutboxMessageEntityTypeConfiguration : IEntityTypeConfiguration<OutboxMessage>
    {
        public void Configure(EntityTypeBuilder<OutboxMessage> builder)
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
