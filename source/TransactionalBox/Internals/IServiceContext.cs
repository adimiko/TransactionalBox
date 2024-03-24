namespace TransactionalBox.Internals
{
    internal interface IServiceContext
    {
        string Id { get; } //TODO or int ?

        string InstanceId { get; } //TODO int
    }
}
