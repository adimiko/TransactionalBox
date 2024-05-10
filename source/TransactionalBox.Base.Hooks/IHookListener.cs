namespace TransactionalBox.Base.Hooks
{
    internal interface IHookListener<T> where T : Hook
    {
        IAsyncEnumerable<DateTime> ListenAsync(CancellationToken cancellationToken);
    }
}
