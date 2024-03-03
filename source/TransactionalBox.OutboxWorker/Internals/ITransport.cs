namespace TransactionalBox.OutboxWorker.Internals
{
    public interface ITransport
    {
        Task Add(string message, string topic);
    }
}
