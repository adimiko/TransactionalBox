using TransactionalBox.Internals;

namespace TransactionalBox.Internals.Inbox.Contexts
{
    internal interface IInboxContext
    {
        string Id { get; }

        string InstanceId { get; }
    }
}
