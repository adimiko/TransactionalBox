using TransactionalBox.Internals;

namespace TransactionalBox.InboxWorker.Internals.Contexts
{
    internal interface IInboxWorkerContext
    {
        string Id { get; }

        string InstanceId { get; }
    }
}
