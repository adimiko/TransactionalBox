namespace TransactionalBox.DistributedLock
{
    public interface IDistributedLock<T>
        where T : Lock, new()
    {
        Task Acquire(string key);

        Task Release();
    }
}
