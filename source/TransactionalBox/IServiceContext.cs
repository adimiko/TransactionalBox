namespace TransactionalBox
{
    public interface IServiceContext
    {
        string Id { get; } //TODO or int ?

        string InstanceId { get; } //TODO int
    }
}
