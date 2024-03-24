namespace TransactionalBox.Internals
{
    internal sealed class ServiceContext : IServiceContext
    {
        public string Id { get; }

        public string InstanceId { get; }

        public ServiceContext(
            string id, 
            string instanceId)
        {
            Id = id;
            InstanceId = instanceId;
        }
    }
}
