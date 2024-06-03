using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TransactionalBox.Internals.Inbox.Storage;

namespace TransactionalBox.EntityFrameworkCore.Internals.Inbox.EntityTypeConfigurations
{
    internal sealed class InboxMessageEntityTypeConfiguration : IEntityTypeConfiguration<InboxMessageStorage>
    {
        public void Configure(EntityTypeBuilder<InboxMessageStorage> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.OccurredUtc);
            builder.Property(x => x.IsProcessed).IsConcurrencyToken();
            builder.Property(x => x.Topic);
            builder.Property(x => x.Payload);
        }
    }
}
