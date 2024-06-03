using TransactionalBox.Internals;

namespace TransactionalBox.Internals.Inbox.Contexts
{
    internal sealed class InboxContext : IInboxContext
    {
        private readonly IServiceContext _serviceContext;

        public string Id => _serviceContext.Id;

        public string InstanceId => _serviceContext.InstanceId;

        public InboxContext(IServiceContext serviceContext) => _serviceContext = serviceContext;

    }
}
