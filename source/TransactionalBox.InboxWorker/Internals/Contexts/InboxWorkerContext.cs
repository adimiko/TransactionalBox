using TransactionalBox.Internals;

namespace TransactionalBox.InboxWorker.Internals.Contexts
{
    internal sealed class InboxWorkerContext : IInboxWorkerContext
    {
        private readonly IServiceContext _serviceContext;

        public string Id => _serviceContext.Id;

        public string InstanceId => _serviceContext.InstanceId;

        public InboxWorkerContext(IServiceContext serviceContext) => _serviceContext = serviceContext;

    }
}
