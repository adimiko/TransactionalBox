namespace TransactionalBox.KeyedSemaphoreSlim
{
    public interface IKeyedSemaphoreSlim
    {
        Task Acquire(string key, CancellationToken cancellationToken = default);

        void Release();
    }
}
