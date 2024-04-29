using TransactionalBox.Internals;

namespace TransactionalBox.Inbox.Internals.Contexts
{
    internal interface IInboxWorkerContext
    {
        string Id { get; }

        string InstanceId { get; }
    }
}
