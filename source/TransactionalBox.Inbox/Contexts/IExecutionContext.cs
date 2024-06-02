namespace TransactionalBox
{
    public interface IExecutionContext
    {
        string Source { get; }

        DateTime OccurredUtc { get; }

        string CorrelationId { get; }

        CancellationToken CancellationToken { get; }
    }
}
