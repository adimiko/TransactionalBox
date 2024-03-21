using TransactionalBox.InboxBase.StorageModel;

namespace TransactionalBox.Inbox.Internals
{
    internal interface IInboxStorage
    {
        Task<InboxMessage?> GetMessage();
    }
}
