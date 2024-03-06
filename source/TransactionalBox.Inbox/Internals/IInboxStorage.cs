using TransactionalBox.InboxBase.StorageModel;

namespace TransactionalBox.Inbox.Internals
{
    public interface IInboxStorage
    {
        Task<InboxMessageStorageModel?> GetMessage();
    }
}
