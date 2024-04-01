using TransactionalBox.Inbox.Contexts;
using TransactionalBox.Inbox.Deserialization;

namespace TransactionalBox.Inbox.Internals
{
    internal sealed class ExecutionContext : IExecutionContext
    {
        public string Source { get;  }

        public DateTime OccurredUtc { get; }

        public string CorrelationId { get; }

        public CancellationToken CancellationToken { get; }

        public ExecutionContext(Metadata metadata, CancellationToken cancellationToken)
        {
            Source = metadata.Source;
            OccurredUtc = metadata.OccurredUtc;
            CorrelationId = metadata.CorrelationId;
            CancellationToken = cancellationToken;
        }
    }
}
