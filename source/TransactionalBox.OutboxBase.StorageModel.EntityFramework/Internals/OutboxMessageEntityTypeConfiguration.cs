using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TransactionalBox.OutboxBase.StorageModel.EntityFramework.Internals
{
    internal sealed class OutboxMessageEntityTypeConfiguration : IEntityTypeConfiguration<OutboxMessage>
    {
        public void Configure(EntityTypeBuilder<OutboxMessage> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.OccurredUtc);
            builder.Property(x => x.ProcessedUtc).IsConcurrencyToken();
            builder.Property(x => x.Topic);
            builder.Property(x => x.Payload);
        }
    }
}
