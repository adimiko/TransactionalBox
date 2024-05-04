using TransactionalBox.Internals;

namespace TransactionalBox.Inbox.Internals.Contexts
{
    internal interface IInboxContext
    {
        string Id { get; }

        string InstanceId { get; }
    }
}
