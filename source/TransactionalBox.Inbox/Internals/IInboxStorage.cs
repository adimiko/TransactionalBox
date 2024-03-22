using TransactionalBox.InboxBase.StorageModel.Internals;

namespace TransactionalBox.Inbox.Internals
{
    internal interface IInboxStorage
    {
        Task<InboxMessage?> GetMessage();
    }
}
