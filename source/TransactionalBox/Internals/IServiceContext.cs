namespace TransactionalBox.Internals
{
    internal interface IServiceContext
    {
        string Id { get; }

        string InstanceId { get; }
    }
}
