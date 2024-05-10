namespace TransactionalBox.Outbox.Internals.Hooks
{
    internal interface IHookListener<T> where T : Hook
    {
        IAsyncEnumerable<DateTime> ListenAsync(CancellationToken cancellationToken);
    }
}
