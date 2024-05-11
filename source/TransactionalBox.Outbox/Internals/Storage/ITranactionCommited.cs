namespace TransactionalBox.Outbox.Internals.Storage
{
    internal interface ITranactionCommited
    {
        Task Commited();
    }
}
